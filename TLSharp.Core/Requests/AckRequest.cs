﻿using System;
using System.Collections.Generic;
using System.IO;
using TeleSharp.TL;

namespace TLSharp.Core.Requests
{
    public class AckRequest : TLMethod
    {
        private readonly List<ulong> _messages;

        public AckRequest(List<ulong> messages)
        {
            _messages = messages;
        }

        public override void SerializeBody(BinaryWriter writer)
        {
            writer.Write(0x62d6b459); // msgs_ack
            writer.Write(0x1cb5c415); // Vector
            writer.Write(_messages.Count);
            foreach (var messageId in _messages)
            {
                writer.Write(messageId);
            }
        }

        public override void DeserializeBody(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeResponse(BinaryReader stream)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => false;
        public override bool Responded { get; }

        public override int Constructor => 0x62d6b459;
    }
}
