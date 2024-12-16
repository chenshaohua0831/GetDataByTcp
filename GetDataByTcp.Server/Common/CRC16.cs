using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Server.Common
{
    public class CRC16
    {
        public static string CRC16_Checkout(string strMsg)
        {
            byte[] puchMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
            ushort crc_reg = 0xFFFF;
            for (int i = 0; i < puchMsg.Length; i++)
            {
                crc_reg = (ushort)((crc_reg >> 8) ^ puchMsg[i]);
                for (int j = 0; j < 8; j++)
                {
                    int check = crc_reg & 0x0001;
                    crc_reg >>= 1;
                    if (check == 0x0001)
                    {
                        crc_reg ^= 0xA001;
                    }
                }
            }

            string crcStr = crc_reg.ToString("X4");
            while (crcStr.Length < 4)
            {
                crcStr = "0" + crcStr;
            }
            return crcStr;
        }
    }
}
