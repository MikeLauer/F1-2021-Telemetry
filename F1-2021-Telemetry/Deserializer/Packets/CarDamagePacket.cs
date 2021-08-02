using System;
using System.Collections.Generic;
using TimHanewich.Toolkit;

namespace Codemasters.F1_2021
{
    public class CarDamagePacket : Packet
    {
        public CarDamageData[] FieldCarStatusData;

        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager bam = new ByteArrayManager(bytes);
            base.LoadBytes(bam.NextBytes(24));


            List<CarDamageData> cdds = new List<CarDamageData>();
            int t = 1;
            for (t = 1; t <= 22; t++)
            {
                cdds.Add(CarDamageData.CreateFromBytes(bam.NextBytes(39)));
            }
            FieldCarStatusData = cdds.ToArray();
        }

        public class CarDamageData
        {
            public byte frontWingDamageLeft = 0;
            public byte frontWingDamageRight = 0;

            public static CarDamageData CreateFromBytes(byte[] bytes)
            {
                CarDamageData ToReturn = new CarDamageData();
                ByteArrayManager bam = new ByteArrayManager(bytes);

                bam.NextBytes(4 * 4);

                bam.NextBytes(4 * 1);

                bam.NextBytes(4 * 1);

                ToReturn.frontWingDamageLeft = bam.NextByte();

                ToReturn.frontWingDamageRight = bam.NextByte();

                return ToReturn;
            }
        }
    }

}