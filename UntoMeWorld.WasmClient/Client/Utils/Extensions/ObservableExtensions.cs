using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using UntoMeWorld.WasmClient.Client.ViewModels;

namespace UntoMeWorld.WasmClient.Client.Utils.Extensions;

public static class ObservableExtensions
{
    /// <summary>
    /// Gets property information for the specified <paramref name="property"/> expression.
    /// </summary>
    /// <typeparam name="TSource">Type of the parameter in the <paramref name="property"/> expression.</typeparam>
    /// <typeparam name="TValue">Type of the property's value.</typeparam>
    /// <param name="property">The expression from which to retrieve the property information.</param>
    /// <returns>Property information for the specified expression.</returns>
    /// <exception cref="ArgumentException">The expression is not understood.</exception>
    private static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> property)
    {
        if (property == null)
        {
            throw new ArgumentNullException(nameof(property));
        }

        if (property.Body is not MemberExpression body)
        {
            throw new ArgumentException("Expression is not a property", nameof(property));
        }

        var propertyInfo = body.Member as PropertyInfo;
        if (propertyInfo == null)
        {
            throw new ArgumentException("Expression is not a property", nameof(property));
        }

        return propertyInfo;
    }

    public static IObservable<TProperty> OnPropChanged<T, TProperty>(this T source,
        Expression<Func<T, TProperty>> property)
        where T : BaseViewModel
    {
        var propertyName = property.GetPropertyInfo().Name;
        var propertySelector = property.Compile();

        return source
            .PropertyChangesStream
            .Where(e => e.EventArgs.PropertyName == propertyName)
            .Select(_ => propertySelector(source));
    }
    public static IObservable<T> OnPropsChanged<T>(
        this T source,
        params Expression<Func<T, object?>>[] properties
    )
        where T : BaseViewModel
    {
        var propertyNames = properties
            .Select(p => p.Body switch
            {
                UnaryExpression unaryExpression => unaryExpression.Operand is MemberExpression expr
                    ? expr.Member.Name
                    : string.Empty,
                MemberExpression memberExpression => memberExpression.Member.Name,
                _ => string.Empty
            })
            .ToHashSet();
        return source
            .PropertyChangesStream
            .Where(e => propertyNames.Contains(e.EventArgs.PropertyName ?? string.Empty))
            .Select(_ => source);
    }
    public static IObservable<TProperty> OnPropertyChangesReplay<T, TProperty>(this T source,
        Expression<Func<T, TProperty>> property)
        where T : INotifyPropertyChanged
    {
        return Observable.Create<TProperty>(o =>
        {
            var propertyName = property.GetPropertyInfo().Name;
            var propertySelector = property.Compile();

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => handler.Invoke,
                    h => source.PropertyChanged += h,
                    h => source.PropertyChanged -= h)
                .Where(e => e.EventArgs.PropertyName == propertyName)
                .Select(_ => propertySelector(source))
                .Replay()
                .Subscribe(o);
        });
    }
    public static IDisposable Subscribe<T>(this IObservable<T> source, Action onNext)
    {
        return source.Subscribe(_ => onNext());
    }
    public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Task onNext)
    {
        return source
            .Select(_ => Observable.FromAsync(async _ => await onNext))
            .Concat()
            .Subscribe();
    }
}