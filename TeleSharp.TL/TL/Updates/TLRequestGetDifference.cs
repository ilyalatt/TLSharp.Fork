using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
namespace TeleSharp.TL.Updates
{
	[TLObject(168039573)]
    public class TLRequestGetDifference : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 168039573;
            }
        }

                public int Pts {get;set;}
        public int Date {get;set;}
        public int Qts {get;set;}
        public Updates.TLAbsDifference Response{ get; set;}


		public void ComputeFlags()
		{
			
		}

        public override void DeserializeBody(BinaryReader br)
        {
            Pts = br.ReadInt32();
Date = br.ReadInt32();
Qts = br.ReadInt32();

        }

        public override void SerializeBody(BinaryWriter bw)
        {
			bw.Write(Constructor);
            bw.Write(Pts);
bw.Write(Date);
bw.Write(Qts);

        }
		public override void DeserializeResponse(BinaryReader br)
		{
			Response = (Updates.TLAbsDifference)ObjectUtils.DeserializeObject(br);

		}
    }
}
