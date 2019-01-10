using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Payments
{
    public sealed class ValidatedRequestedInfo : ITlType, IEquatable<ValidatedRequestedInfo>, IComparable<ValidatedRequestedInfo>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0xd1451883;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public Option<string> Id { get; }
            public Option<Arr<T.ShippingOption>> ShippingOptions { get; }
            
            public Tag(
                Option<string> id,
                Option<Arr<T.ShippingOption>> shippingOptions
            ) {
                Id = id;
                ShippingOptions = shippingOptions;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(MaskBit(0, Id) | MaskBit(1, ShippingOptions), bw, WriteInt);
                Write(Id, bw, WriteOption<string>(WriteString));
                Write(ShippingOptions, bw, WriteOption<Arr<T.ShippingOption>>(WriteVector<T.ShippingOption>(WriteSerializable)));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var flags = Read(br, ReadInt);
                var id = Read(br, ReadOption(flags, 0, ReadString));
                var shippingOptions = Read(br, ReadOption(flags, 1, ReadVector(T.ShippingOption.Deserialize)));
                return new Tag(id, shippingOptions);
            }
        }

        readonly ITlTypeTag _tag;
        ValidatedRequestedInfo(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator ValidatedRequestedInfo(Tag tag) => new ValidatedRequestedInfo(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static ValidatedRequestedInfo Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (ValidatedRequestedInfo) Tag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { Tag.TypeNumber });
            }
        }

        T Match<T>(
            Func<T> _,
            Func<Tag, T> tag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case Tag x when tag != null: return tag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<Tag, T> tag
        ) => Match(
            () => throw new Exception("WTF"),
            tag ?? throw new ArgumentNullException(nameof(tag))
        );

        public bool Equals(ValidatedRequestedInfo other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is ValidatedRequestedInfo x && Equals(x);
        public static bool operator ==(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(ValidatedRequestedInfo other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is ValidatedRequestedInfo x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => a.CompareTo(b) <= 0;
        public static bool operator <(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => a.CompareTo(b) < 0;
        public static bool operator >(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => a.CompareTo(b) > 0;
        public static bool operator >=(ValidatedRequestedInfo a, ValidatedRequestedInfo b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}