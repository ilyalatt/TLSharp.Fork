using System;
using System.IO;
using BigMath;
using LanguageExt;
using static TLSharp.Rpc.TlMarshal;
using T = TLSharp.Rpc.Types;

namespace TLSharp.Rpc.Types.Payments
{
    public sealed class PaymentForm : ITlType, IEquatable<PaymentForm>, IComparable<PaymentForm>, IComparable
    {
        public sealed class Tag : ITlTypeTag, IEquatable<Tag>, IComparable<Tag>, IComparable
        {
            internal const uint TypeNumber = 0x3f56aea3;
            uint ITlTypeTag.TypeNumber => TypeNumber;
            
            public readonly bool CanSaveCredentials;
            public readonly bool PasswordMissing;
            public readonly int BotId;
            public readonly T.Invoice Invoice;
            public readonly int ProviderId;
            public readonly string Url;
            public readonly Option<string> NativeProvider;
            public readonly Option<T.DataJson> NativeParams;
            public readonly Option<T.PaymentRequestedInfo> SavedInfo;
            public readonly Option<T.PaymentSavedCredentials> SavedCredentials;
            public readonly Arr<T.User> Users;
            
            public Tag(
                bool canSaveCredentials,
                bool passwordMissing,
                int botId,
                Some<T.Invoice> invoice,
                int providerId,
                Some<string> url,
                Option<string> nativeProvider,
                Option<T.DataJson> nativeParams,
                Option<T.PaymentRequestedInfo> savedInfo,
                Option<T.PaymentSavedCredentials> savedCredentials,
                Some<Arr<T.User>> users
            ) {
                CanSaveCredentials = canSaveCredentials;
                PasswordMissing = passwordMissing;
                BotId = botId;
                Invoice = invoice;
                ProviderId = providerId;
                Url = url;
                NativeProvider = nativeProvider;
                NativeParams = nativeParams;
                SavedInfo = savedInfo;
                SavedCredentials = savedCredentials;
                Users = users;
            }
            
            (bool, bool, int, T.Invoice, int, string, Option<string>, Option<T.DataJson>, Option<T.PaymentRequestedInfo>, Option<T.PaymentSavedCredentials>, Arr<T.User>) CmpTuple =>
                (CanSaveCredentials, PasswordMissing, BotId, Invoice, ProviderId, Url, NativeProvider, NativeParams, SavedInfo, SavedCredentials, Users);

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

            public override string ToString() => $"(CanSaveCredentials: {CanSaveCredentials}, PasswordMissing: {PasswordMissing}, BotId: {BotId}, Invoice: {Invoice}, ProviderId: {ProviderId}, Url: {Url}, NativeProvider: {NativeProvider}, NativeParams: {NativeParams}, SavedInfo: {SavedInfo}, SavedCredentials: {SavedCredentials}, Users: {Users})";
            
            
            void ITlSerializable.Serialize(BinaryWriter bw)
            {
                Write(MaskBit(2, CanSaveCredentials) | MaskBit(3, PasswordMissing) | MaskBit(4, NativeProvider) | MaskBit(4, NativeParams) | MaskBit(0, SavedInfo) | MaskBit(1, SavedCredentials), bw, WriteInt);
                Write(BotId, bw, WriteInt);
                Write(Invoice, bw, WriteSerializable);
                Write(ProviderId, bw, WriteInt);
                Write(Url, bw, WriteString);
                Write(NativeProvider, bw, WriteOption<string>(WriteString));
                Write(NativeParams, bw, WriteOption<T.DataJson>(WriteSerializable));
                Write(SavedInfo, bw, WriteOption<T.PaymentRequestedInfo>(WriteSerializable));
                Write(SavedCredentials, bw, WriteOption<T.PaymentSavedCredentials>(WriteSerializable));
                Write(Users, bw, WriteVector<T.User>(WriteSerializable));
            }
            
            internal static Tag DeserializeTag(BinaryReader br)
            {
                var flags = Read(br, ReadInt);
                var canSaveCredentials = Read(br, ReadOption(flags, 2));
                var passwordMissing = Read(br, ReadOption(flags, 3));
                var botId = Read(br, ReadInt);
                var invoice = Read(br, T.Invoice.Deserialize);
                var providerId = Read(br, ReadInt);
                var url = Read(br, ReadString);
                var nativeProvider = Read(br, ReadOption(flags, 4, ReadString));
                var nativeParams = Read(br, ReadOption(flags, 4, T.DataJson.Deserialize));
                var savedInfo = Read(br, ReadOption(flags, 0, T.PaymentRequestedInfo.Deserialize));
                var savedCredentials = Read(br, ReadOption(flags, 1, T.PaymentSavedCredentials.Deserialize));
                var users = Read(br, ReadVector(T.User.Deserialize));
                return new Tag(canSaveCredentials, passwordMissing, botId, invoice, providerId, url, nativeProvider, nativeParams, savedInfo, savedCredentials, users);
            }
        }

        readonly ITlTypeTag _tag;
        PaymentForm(ITlTypeTag tag) => _tag = tag ?? throw new ArgumentNullException(nameof(tag));

        public static explicit operator PaymentForm(Tag tag) => new PaymentForm(tag);

        void ITlSerializable.Serialize(BinaryWriter bw)
        {
            WriteUint(bw, _tag.TypeNumber);
            _tag.Serialize(bw);
        }

        internal static PaymentForm Deserialize(BinaryReader br)
        {
            var typeNumber = ReadUint(br);
            switch (typeNumber)
            {
                case Tag.TypeNumber: return (PaymentForm) Tag.DeserializeTag(br);
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

        public bool Equals(PaymentForm other) => !ReferenceEquals(other, null) && (ReferenceEquals(this, other) || CmpPair == other.CmpPair);
        public override bool Equals(object other) => other is PaymentForm x && Equals(x);
        public static bool operator ==(PaymentForm x, PaymentForm y) => x?.Equals(y) ?? ReferenceEquals(y, null);
        public static bool operator !=(PaymentForm x, PaymentForm y) => !(x == y);

        public int CompareTo(PaymentForm other) => ReferenceEquals(other, null) ? throw new ArgumentNullException(nameof(other)) : ReferenceEquals(this, other) ? 0 : CmpPair.CompareTo(other.CmpPair);
        int IComparable.CompareTo(object other) => other is PaymentForm x ? CompareTo(x) : throw new ArgumentException("bad type", nameof(other));
        public static bool operator <=(PaymentForm x, PaymentForm y) => x.CompareTo(y) <= 0;
        public static bool operator <(PaymentForm x, PaymentForm y) => x.CompareTo(y) < 0;
        public static bool operator >(PaymentForm x, PaymentForm y) => x.CompareTo(y) > 0;
        public static bool operator >=(PaymentForm x, PaymentForm y) => x.CompareTo(y) >= 0;

        public override int GetHashCode() => CmpPair.GetHashCode();

        public override string ToString() => $"PaymentForm.{_tag.GetType().Name}{_tag}";
    }
}