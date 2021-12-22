﻿using System;

namespace UntoMeWorld.Domain.Model
{
    public class Child : IModel, IPerson
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int Grade { get; set; }
        public string ChurchId { get; set; }
        public int ShoeSize { get; set; }
        public int TopSize { get; set; }
        public int WaistSize { get; set; }
        public UnderwearSize UnderwearSize { get; set; }
        public UnderwearSize BraSize { get; set; }
        public int UniformsCount { get; set; }
        public bool ReceivesChristmasGift { get; set; }
        public int ReceivesUniforms { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Grade)}: {Grade}, {nameof(ChurchId)}: {ChurchId}, {nameof(ShoeSize)}: {ShoeSize}, {nameof(TopSize)}: {TopSize}, {nameof(WaistSize)}: {WaistSize}, {nameof(UnderwearSize)}: {UnderwearSize}, {nameof(BraSize)}: {BraSize}, {nameof(UniformsCount)}: {UniformsCount}, {nameof(ReceivesChristmasGift)}: {ReceivesChristmasGift}, {nameof(ReceivesUniforms)}: {ReceivesUniforms}, {nameof(Name)}: {Name}, {nameof(Lastname)}: {Lastname}, {nameof(Age)}: {Age}, {nameof(Gender)}: {Gender}";
        }
    }
}