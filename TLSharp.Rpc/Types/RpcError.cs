using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class RpcError : ITlType, IEquatable<RpcError>, IComparable<RpcError>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0x2144ca19;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly int ErrorCode;
            public readonly string ErrorMessage;
            
            public Tag(
                int errorCode,
                Some<string> errorMessage
            ) {
                ErrorCode = errorCode;
                ErrorMessage = errorMessage;
            }
            
            (int, string) CmpTuple =>
                (ErrorCode, ErrorMessage);

            public bool Equals(Tag other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpTuple == other.CmpTuple);
            public override bool Equals(object other) => other is Tag x && Equals(x);
            public static bool operator ==(Tag x, Tag y) => x?.Equals(y) ?? ReferenceEquals(y, null);
            public static bool operator !=(Tag x, Tag y) => !(x == y);

            public int CompareTo(Tag other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpTuple.CompareTo(other.CmpTuple);
            int IComparable.CompareTo(object other) => other is Tag x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
            public static bool operator <=(Tag x, Tag y) => x.CompareTo(y) <= 0;
            public static bool operator <(Tag x, Tag y) => x.CompareTo(y) < 0;
            public static bool operator >(Tag x, Tag y) => x.CompareTo(y) > 0;
            public static bool operator >=(Tag x, Tag y) => x.CompareTo(y) >= 0;

            public override int GetHashCode() => CmpTuple.GetHashCode();

            public override string ToString() => $"(ErrorCode: {ErrorCode}, ErrorMessage: {ErrorMessage})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(ErrorCode, bw, WriteInt);
                Write(ErrorMessage, bw, WriteString);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var errorCode = Read(br, ReadInt);
                var errorMessage = Read(br, ReadString);
                return new Tag(errorCode, errorMessage);
            }
        }

        readonly ITlTypeTag _tag;
        RpcError(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator RpcError(Tag tag) => new RpcError(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static RpcError Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (RpcError) Tag.DeserializeTag(br);
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

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public bool Equals(RpcError other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is RpcError x && Equals(x);
        public static bool operator ==(RpcError x, RpcError y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(RpcError x, RpcError y) => !(x == y);

        public int CompareTo(RpcError other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is RpcError x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(RpcError x, RpcError y) => x.CompareTo(y) <= 0;
        public static bool operator <(RpcError x, RpcError y) => x.CompareTo(y) < 0;
        public static bool operator >(RpcError x, RpcError y) => x.CompareTo(y) > 0;
        public static bool operator >=(RpcError x, RpcError y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"RpcError.{_tag.GetType().Name}{_tag}";
    }
}