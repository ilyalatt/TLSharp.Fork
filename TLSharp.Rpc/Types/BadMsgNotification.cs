using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class BadMsgNotification : ITlType, IEquatable<BadMsgNotification>, IComparable<BadMsgNotification>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0xa7eff811;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly long BadMsgId;
            public readonly int BadMsgSeqno;
            public readonly int ErrorCode;
            
            public Tag(
                long badMsgId,
                int badMsgSeqno,
                int errorCode
            ) {
                BadMsgId = badMsgId;
                BadMsgSeqno = badMsgSeqno;
                ErrorCode = errorCode;
            }
            
            (long, int, int) CmpTuple =>
                (BadMsgId, BadMsgSeqno, ErrorCode);

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

            public override string ToString() => $"(BadMsgId: {BadMsgId}, BadMsgSeqno: {BadMsgSeqno}, ErrorCode: {ErrorCode})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(BadMsgId, bw, WriteLong);
                Write(BadMsgSeqno, bw, WriteInt);
                Write(ErrorCode, bw, WriteInt);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var badMsgId = Read(br, ReadLong);
                var badMsgSeqno = Read(br, ReadInt);
                var errorCode = Read(br, ReadInt);
                return new Tag(badMsgId, badMsgSeqno, errorCode);
            }
        }

        public sealed class ServerSaltTag : ITlTypeTag, IEquatable<ServerSaltTag>, IComparable<ServerSaltTag>, IComparable
        {
            internal const uint TypeNumber = 0xedab447b;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly long BadMsgId;
            public readonly int BadMsgSeqno;
            public readonly int ErrorCode;
            public readonly long NewServerSalt;
            
            public ServerSaltTag(
                long badMsgId,
                int badMsgSeqno,
                int errorCode,
                long newServerSalt
            ) {
                BadMsgId = badMsgId;
                BadMsgSeqno = badMsgSeqno;
                ErrorCode = errorCode;
                NewServerSalt = newServerSalt;
            }
            
            (long, int, int, long) CmpTuple =>
                (BadMsgId, BadMsgSeqno, ErrorCode, NewServerSalt);

            public bool Equals(ServerSaltTag other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpTuple == other.CmpTuple);
            public override bool Equals(object other) => other is ServerSaltTag x && Equals(x);
            public static bool operator ==(ServerSaltTag x, ServerSaltTag y) => x?.Equals(y) ?? ReferenceEquals(y, null);
            public static bool operator !=(ServerSaltTag x, ServerSaltTag y) => !(x == y);

            public int CompareTo(ServerSaltTag other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpTuple.CompareTo(other.CmpTuple);
            int IComparable.CompareTo(object other) => other is ServerSaltTag x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
            public static bool operator <=(ServerSaltTag x, ServerSaltTag y) => x.CompareTo(y) <= 0;
            public static bool operator <(ServerSaltTag x, ServerSaltTag y) => x.CompareTo(y) < 0;
            public static bool operator >(ServerSaltTag x, ServerSaltTag y) => x.CompareTo(y) > 0;
            public static bool operator >=(ServerSaltTag x, ServerSaltTag y) => x.CompareTo(y) >= 0;

            public override int GetHashCode() => CmpTuple.GetHashCode();

            public override string ToString() => $"(BadMsgId: {BadMsgId}, BadMsgSeqno: {BadMsgSeqno}, ErrorCode: {ErrorCode}, NewServerSalt: {NewServerSalt})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(BadMsgId, bw, WriteLong);
                Write(BadMsgSeqno, bw, WriteInt);
                Write(ErrorCode, bw, WriteInt);
                Write(NewServerSalt, bw, WriteLong);
            }
            
            internal static ServerSaltTag DeserializeTag(BinaryReader br)
            {
                var badMsgId = Read(br, ReadLong);
                var badMsgSeqno = Read(br, ReadInt);
                var errorCode = Read(br, ReadInt);
                var newServerSalt = Read(br, ReadLong);
                return new ServerSaltTag(badMsgId, badMsgSeqno, errorCode, newServerSalt);
            }
        }

        readonly ITlTypeTag _tag;
        BadMsgNotification(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator BadMsgNotification(Tag tag) => new BadMsgNotification(tag);
        public static explicit operator BadMsgNotification(ServerSaltTag tag) => new BadMsgNotification(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static BadMsgNotification Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (BadMsgNotification) Tag.DeserializeTag(br);
                case ServerSaltTag.TypeNumber: return (BadMsgNotification) ServerSaltTag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { Tag.TypeNumber, ServerSaltTag.TypeNumber });
            }
        }

        public T Match<T>(
            Func<T> _,
            Func<Tag, T> tag = null,
            Func<ServerSaltTag, T> serverSaltTag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case Tag x when tag != null: return tag(x);
                case ServerSaltTag x when serverSaltTag != null: return serverSaltTag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<Tag, T> tag,
            Func<ServerSaltTag, T> serverSaltTag
        ) => Match(
            () => throw new Exception("WTF"),
            tag ?? throw new ArgumentNullException(nameof(tag)),
            serverSaltTag ?? throw new ArgumentNullException(nameof(serverSaltTag))
        );

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                case ServerSaltTag _: return 1;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public bool Equals(BadMsgNotification other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is BadMsgNotification x && Equals(x);
        public static bool operator ==(BadMsgNotification x, BadMsgNotification y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(BadMsgNotification x, BadMsgNotification y) => !(x == y);

        public int CompareTo(BadMsgNotification other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is BadMsgNotification x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(BadMsgNotification x, BadMsgNotification y) => x.CompareTo(y) <= 0;
        public static bool operator <(BadMsgNotification x, BadMsgNotification y) => x.CompareTo(y) < 0;
        public static bool operator >(BadMsgNotification x, BadMsgNotification y) => x.CompareTo(y) > 0;
        public static bool operator >=(BadMsgNotification x, BadMsgNotification y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"BadMsgNotification.{_tag.GetType().Name}{_tag}";
    }
}