using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Contacts
{
    public sealed class ImportedContacts : ITlType, IEquatable<ImportedContacts>, IComparable<ImportedContacts>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0x77d01c3b;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly Arr<T.ImportedContact> Imported;
            public readonly Arr<T.PopularContact> PopularInvites;
            public readonly Arr<long> RetryContacts;
            public readonly Arr<T.User> Users;
            
            public Tag(
                Some<Arr<T.ImportedContact>> imported,
                Some<Arr<T.PopularContact>> popularInvites,
                Some<Arr<long>> retryContacts,
                Some<Arr<T.User>> users
            ) {
                Imported = imported;
                PopularInvites = popularInvites;
                RetryContacts = retryContacts;
                Users = users;
            }
            
            (Arr<T.ImportedContact>, Arr<T.PopularContact>, Arr<long>, Arr<T.User>) CmpTuple =>
                (Imported, PopularInvites, RetryContacts, Users);

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

            public override string ToString() => $"(Imported: {Imported}, PopularInvites: {PopularInvites}, RetryContacts: {RetryContacts}, Users: {Users})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(Imported, bw, WriteVector<T.ImportedContact>(WriteSerializable));
                Write(PopularInvites, bw, WriteVector<T.PopularContact>(WriteSerializable));
                Write(RetryContacts, bw, WriteVector<long>(WriteLong));
                Write(Users, bw, WriteVector<T.User>(WriteSerializable));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var imported = Read(br, ReadVector(T.ImportedContact.Deserialize));
                var popularInvites = Read(br, ReadVector(T.PopularContact.Deserialize));
                var retryContacts = Read(br, ReadVector(ReadLong));
                var users = Read(br, ReadVector(T.User.Deserialize));
                return new Tag(imported, popularInvites, retryContacts, users);
            }
        }

        readonly ITlTypeTag _tag;
        ImportedContacts(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator ImportedContacts(Tag tag) => new ImportedContacts(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static ImportedContacts Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (ImportedContacts) Tag.DeserializeTag(br);
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

        public bool Equals(ImportedContacts other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is ImportedContacts x && Equals(x);
        public static bool operator ==(ImportedContacts x, ImportedContacts y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(ImportedContacts x, ImportedContacts y) => !(x == y);

        public int CompareTo(ImportedContacts other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is ImportedContacts x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(ImportedContacts x, ImportedContacts y) => x.CompareTo(y) <= 0;
        public static bool operator <(ImportedContacts x, ImportedContacts y) => x.CompareTo(y) < 0;
        public static bool operator >(ImportedContacts x, ImportedContacts y) => x.CompareTo(y) > 0;
        public static bool operator >=(ImportedContacts x, ImportedContacts y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"ImportedContacts.{_tag.GetType().Name}{_tag}";
    }
}