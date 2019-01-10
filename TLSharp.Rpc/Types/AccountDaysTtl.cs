using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class AccountDaysTtl : ITlType, IEquatable<AccountDaysTtl>, IComparable<AccountDaysTtl>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0xb8d0afdf;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public int Days { get; }
            
            public Tag(
                int days
            ) {
                Days = days;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Days, bw, WriteInt);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var days = Read(br, ReadInt);
                return new Tag(days);
            }
        }

        readonly ITlTypeTag _tag;
        AccountDaysTtl(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator AccountDaysTtl(Tag tag) => new AccountDaysTtl(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static AccountDaysTtl Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (AccountDaysTtl) Tag.DeserializeTag(br);
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

        public bool Equals(AccountDaysTtl other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is AccountDaysTtl x && Equals(x);
        public static bool operator ==(AccountDaysTtl a, AccountDaysTtl b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(AccountDaysTtl a, AccountDaysTtl b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(AccountDaysTtl other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is AccountDaysTtl x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(AccountDaysTtl a, AccountDaysTtl b) => a.CompareTo(b) <= 0;
        public static bool operator <(AccountDaysTtl a, AccountDaysTtl b) => a.CompareTo(b) < 0;
        public static bool operator >(AccountDaysTtl a, AccountDaysTtl b) => a.CompareTo(b) > 0;
        public static bool operator >=(AccountDaysTtl a, AccountDaysTtl b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}