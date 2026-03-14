using System;
using System.IO;
using System.Text;

namespace SerialUpgrader
{
    /// <summary>
    /// Parses the 256-byte magic header that sits at the start of a .xbin file.
    /// Layout matches device-side magic_header_t (bitmask + reserved1[6] ...).
    /// </summary>
    public sealed class MagicHeader
    {
        private const uint MAGIC_VALUE = 0x4D414749u; // 'MAGI'
        private const int HEADER_SIZE = 256;
        private const int CRC_COVERS_UPTO = 252;      // bytes [0..251] covered by ThisCrc32

        // Parsed fields
        public uint DataType { get; private set; }
        public uint DataOffset { get; private set; }
        public uint DataAddress { get; private set; }
        public uint DataLength { get; private set; }
        public uint DataCrc32 { get; private set; }
        public string Version { get; private set; } = string.Empty;
        public uint ThisAddress { get; private set; }

        // Optional raw pieces (not currently used, but kept for completeness)
        public uint Bitmask { get; private set; }

        public static MagicHeader Parse(ReadOnlySpan<byte> span)
        {
            if (span.Length < HEADER_SIZE)
                throw new ArgumentException("Header too short.");

            uint magic = ReadU32(span, 0);
            if (magic != MAGIC_VALUE)
                throw new InvalidDataException("MAGI missing.");

            // New layout (device / new gen_magic_header.py)
            uint bitmask = ReadU32(span, 4);
            // reserved1[6] skip 24 bytes: offsets 8..31
            uint dataType = ReadU32(span, 32);
            uint dataOffset = ReadU32(span, 36);
            uint dataAddress = ReadU32(span, 40);
            uint dataLength = ReadU32(span, 44);
            uint dataCrc32 = ReadU32(span, 48);
            // reserved2[11] skip to 96
            string version = ReadAscii(span.Slice(96, 128));
            // reserved3[6] skip to 248
            uint thisAddress = ReadU32(span, 248);
            uint thisCrc32 = ReadU32(span, 252);

            uint calc = Crc32(span.Slice(0, CRC_COVERS_UPTO));
            if (calc != thisCrc32)
                throw new InvalidDataException("this_crc32 mismatch.");

            return new MagicHeader
            {
                Bitmask = bitmask,
                DataType = dataType,
                DataOffset = dataOffset,
                DataAddress = dataAddress,
                DataLength = dataLength,
                DataCrc32 = dataCrc32,
                Version = version,
                ThisAddress = thisAddress,
            };
        }

        // Helpers
        private static uint ReadU32(ReadOnlySpan<byte> s, int offset)
            => BitConverter.ToUInt32(s.Slice(offset, 4));

        private static string ReadAscii(ReadOnlySpan<byte> s)
        {
            int zero = s.IndexOf((byte)0);
            if (zero >= 0) s = s.Slice(0, zero);
            return Encoding.ASCII.GetString(s);
        }

        // Standard IEEE CRC32 (poly 0xEDB88320)
        private static uint Crc32(ReadOnlySpan<byte> data)
        {
            uint crc = 0xFFFFFFFF;
            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    bool lsb = (crc & 1) != 0;
                    crc >>= 1;
                    if (lsb) crc ^= 0xEDB88320u;
                }
            }
            return crc ^ 0xFFFFFFFF;
        }
    }
}