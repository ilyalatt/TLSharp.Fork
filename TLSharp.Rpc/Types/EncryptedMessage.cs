using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types
{
    public sealed class EncryptedMessage : ITlType, IEquatable<EncryptedMessage>, IComparable<EncryptedMessage>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0xed18c118;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly long RandomId;
            public readonly int ChatId;
            public readonly int Date;
            public readonly Arr<byte> Bytes;
            public readonly T.EncryptedFile File;
            
            public Tag(
                long randomId,
                int chatId,
                int date,
                Some<Arr<byte>> bytes,
                Some<T.EncryptedFile> file
            ) {
                RandomId = randomId;
                ChatId = chatId;
                Date = date;
                Bytes = bytes;
                File = file;
            }
            
            (long, int, int, Arr<byte>, T.EncryptedFile) CmpTuple =>
                (RandomId, ChatId, Date, Bytes, File);

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

            public override string ToString() => $"(RandomId: {RandomId}, ChatId: {ChatId}, Date: {Date}, Bytes: {Bytes}, File: {File})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(RandomId, bw, WriteLong);
                Write(ChatId, bw, WriteInt);
                Write(Date, bw, WriteInt);
                Write(Bytes, bw, WriteBytes);
                Write(File, bw, WriteSerializable);
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var randomId = Read(br, ReadLong);
                var chatId = Read(br, ReadInt);
                var date = Read(br, ReadInt);
                var bytes = Read(br, ReadBytes);
                var file = Read(br, T.EncryptedFile.Deserialize);
                return new Tag(randomId, chatId, date, bytes, file);
            }
        }

        public sealed class ServiceTag : ITlTypeTag, IEquatable<ServiceTag>, IComparable<ServiceTag>, IComparable
        {
            internal const uint TypeNumber = 0x23734b06;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly long RandomId;
            public readonly int ChatId;
            public readonly int Date;
            public readonly Arr<byte> Bytes;
            
            public ServiceTag(
                long randomId,
                int chatId,
                int date,
                Some<Arr<byte>> bytes
            ) {
                RandomId = randomId;
                ChatId = chatId;
                Date = date;
                Bytes = bytes;
            }
            
            (long, int, int, Arr<byte>) CmpTuple =>
                (RandomId, ChatId, Date, Bytes);

            public bool Equals(ServiceTag other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpTuple == other.CmpTuple);
            public override bool Equals(object other) => other is ServiceTag x && Equals(x);
            public static bool operator ==(ServiceTag x, ServiceTag y) => x?.Equals(y) ?? ReferenceEquals(y, null);
            public static bool operator !=(ServiceTag x, ServiceTag y) => !(x == y);

            public int CompareTo(ServiceTag other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpTuple.CompareTo(other.CmpTuple);
            int IComparable.CompareTo(object other) => other is ServiceTag x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
            public static bool operator <=(ServiceTag x, ServiceTag y) => x.CompareTo(y) <= 0;
            public static bool operator <(ServiceTag x, ServiceTag y) => x.CompareTo(y) < 0;
            public static bool operator >(ServiceTag x, ServiceTag y) => x.CompareTo(y) > 0;
            public static bool operator >=(ServiceTag x, ServiceTag y) => x.CompareTo(y) >= 0;

            public override int GetHashCode() => CmpTuple.GetHashCode();

            public override string ToString() => $"(RandomId: {RandomId}, ChatId: {ChatId}, Date: {Date}, Bytes: {Bytes})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(RandomId, bw, WriteLong);
                Write(ChatId, bw, WriteInt);
                Write(Date, bw, WriteInt);
                Write(Bytes, bw, WriteBytes);
            }
            
            internal static ServiceTag DeserializeTag(BinaryReader br)
            {
                var randomId = Read(br, ReadLong);
                var chatId = Read(br, ReadInt);
                var date = Read(br, ReadInt);
                var bytes = Read(br, ReadBytes);
                return new ServiceTag(randomId, chatId, date, bytes);
            }
        }

        readonly ITlTypeTag _tag;
        EncryptedMessage(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator EncryptedMessage(Tag tag) => new EncryptedMessage(tag);
        public static explicit operator EncryptedMessage(ServiceTag tag) => new EncryptedMessage(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static EncryptedMessage Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (EncryptedMessage) Tag.DeserializeTag(br);
                case ServiceTag.TypeNumber: return (EncryptedMessage) ServiceTag.DeserializeTag(br);
                default: throw TlRpcDeserializeException.UnexpectedTypeNumber(actual: typeNumber, expected: new[] { Tag.TypeNumber, ServiceTag.TypeNumber });
            }
        }

        public T Match<T>(
            Func<T> _,
            Func<Tag, T> tag = null,
            Func<ServiceTag, T> serviceTag = null
        ) {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            switch (_tag)
            {
                case Tag x when tag != null: return tag(x);
                case ServiceTag x when serviceTag != null: return serviceTag(x);
                default: return _();
            }
        }

        public T Match<T>(
            Func<Tag, T> tag,
            Func<ServiceTag, T> serviceTag
        ) => Match(
            () => throw new Exception("WTF"),
            tag ?? throw new ArgumentNullException(nameof(tag)),
            serviceTag ?? throw new ArgumentNullException(nameof(serviceTag))
        );

        int GetTagOrder()
        {
            switch (_tag)
            {
                case Tag _: return 0;
                case ServiceTag _: return 1;
                default: throw new Exception("WTF");
            }
        }
        (int, object) CmpPair => (GetTagOrder(), _tag);

        public bool Equals(EncryptedMessage other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is EncryptedMessage x && Equals(x);
        public static bool operator ==(EncryptedMessage x, EncryptedMessage y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(EncryptedMessage x, EncryptedMessage y) => !(x == y);

        public int CompareTo(EncryptedMessage other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is EncryptedMessage x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(EncryptedMessage x, EncryptedMessage y) => x.CompareTo(y) <= 0;
        public static bool operator <(EncryptedMessage x, EncryptedMessage y) => x.CompareTo(y) < 0;
        public static bool operator >(EncryptedMessage x, EncryptedMessage y) => x.CompareTo(y) > 0;
        public static bool operator >=(EncryptedMessage x, EncryptedMessage y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"EncryptedMessage.{_tag.GetType().Name}{_tag}";
    }
}