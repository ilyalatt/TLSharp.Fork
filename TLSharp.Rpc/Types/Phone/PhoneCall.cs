using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Phone
{
    public sealed class PhoneCall : ITlType, IEquatable<PhoneCall>, IComparable<PhoneCall>, IComparable
    {
        public sealed class Tag : Record<Tag>, ITlTypeTag
        {
            internal const uint TypeNumber = 0xec82e140;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public T.PhoneCall PhoneCall { get; }
            public Arr<T.User> Users { get; }
            
            public Tag(
                Some<T.PhoneCall> phoneCall,
                Some<Arr<T.User>> users
            ) {
                PhoneCall = phoneCall;
                Users = users;
            }
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(PhoneCall, bw, WriteSerializable);
                Write(Users, bw, WriteVector<T.User>(WriteSerializable));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var phoneCall = Read(br, T.PhoneCall.Deserialize);
                var users = Read(br, ReadVector(T.User.Deserialize));
                return new Tag(phoneCall, users);
            }
        }

        readonly ITlTypeTag _tag;
        PhoneCall(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator PhoneCall(Tag tag) => new PhoneCall(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static PhoneCall Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (PhoneCall) Tag.DeserializeTag(br);
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

        public bool Equals(PhoneCall other) => !ReferenceEquals(other, null) && _tag.Equals(other._tag);
        public override bool Equals(object obj) => obj is PhoneCall x && Equals(x);
        public static bool operator ==(PhoneCall a, PhoneCall b) => a?.Equals(b) ?? ReferenceEquals(b, null);
        public static bool operator !=(PhoneCall a, PhoneCall b) => !(a == b);

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public int CompareTo(PhoneCall other) => !ReferenceEquals(other, null) ? CmpPair.CompareTo(other.CmpPair) : throw new ArgumentNullException(nameof(other));
        int IComparable.CompareTo(object other) => other is PhoneCall x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(PhoneCall a, PhoneCall b) => a.CompareTo(b) <= 0;
        public static bool operator <(PhoneCall a, PhoneCall b) => a.CompareTo(b) < 0;
        public static bool operator >(PhoneCall a, PhoneCall b) => a.CompareTo(b) > 0;
        public static bool operator >=(PhoneCall a, PhoneCall b) => a.CompareTo(b) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();
    }
}