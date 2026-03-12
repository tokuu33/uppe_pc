using System;
using System.Diagnostics;
using System.Text;

namespace SerialUpgrader;

public enum MagicHeaderDataType : uint
{
    Application = 0,
}

public class MagicHeader
{
    public uint Magic { get; set; } = 0x4D414749;
    public uint UpdateFlag { get; set; }     // 新增：升级标志
    public uint RollbackFlag { get; set; }   // 新增：回滚标志
    public uint BootFailCount { get; set; }  // 新增：失败计数

    public MagicHeaderDataType DataType { get; set; }
    public uint DataOffset { get; set; }
    public uint DataAddress { get; set; }
    public uint DataLength { get; set; }
    public uint DataCrc32 { get; set; }

    public uint NewAppLength { get; set; }   // 新增：新固件长度
    public uint NewAppCrc32 { get; set; }    // 新增：新固件校验和

    public uint BackupLength { get; set; }   // 新增：备份长度
    public uint BackupCrc32 { get; set; }    // 新增：备份校验和

    public string Version { get; set; } = string.Empty;
    public uint ThisAddress { get; set; } = 0x0800C000;
    public uint ThisCrc32 { get; set; }

    public static MagicHeader? Parse(byte[] xbin)
    {
        if (xbin.Length < 256) return null;
        uint magic = BitConverter.ToUInt32(xbin, 0);
        if (magic != 0x4D414749) return null;

        MagicHeader header = new MagicHeader
        {
            Magic = magic,
            UpdateFlag = BitConverter.ToUInt32(xbin, 4),
            RollbackFlag = BitConverter.ToUInt32(xbin, 8),
            BootFailCount = BitConverter.ToUInt32(xbin, 12),
            DataType = (MagicHeaderDataType)BitConverter.ToUInt32(xbin, 16),
            DataOffset = BitConverter.ToUInt32(xbin, 20),
            DataAddress = BitConverter.ToUInt32(xbin, 24),
            DataLength = BitConverter.ToUInt32(xbin, 28),
            DataCrc32 = BitConverter.ToUInt32(xbin, 32),
            NewAppLength = BitConverter.ToUInt32(xbin, 36),
            NewAppCrc32 = BitConverter.ToUInt32(xbin, 40),
            BackupLength = BitConverter.ToUInt32(xbin, 44),
            BackupCrc32 = BitConverter.ToUInt32(xbin, 48),
            Version = Encoding.ASCII.GetString(xbin, 52, 128).TrimEnd('\0'),
            ThisAddress = BitConverter.ToUInt32(xbin, 248),
            ThisCrc32 = BitConverter.ToUInt32(xbin, 252)
        };

        // 你原来的打包脚本可能是按旧结构生成的，如果是旧固件，你需要重新对齐解析逻辑
        // 此处假设打包脚本生成的 xbin 也已更新对齐到新结构
        return header;
    }

    public byte[] ToBytes()
    {
        byte[] bytes = new byte[256];
        BitConverter.GetBytes(Magic).CopyTo(bytes, 0);
        BitConverter.GetBytes(UpdateFlag).CopyTo(bytes, 4);
        BitConverter.GetBytes(RollbackFlag).CopyTo(bytes, 8);
        BitConverter.GetBytes(BootFailCount).CopyTo(bytes, 12);

        BitConverter.GetBytes((uint)DataType).CopyTo(bytes, 16);
        BitConverter.GetBytes(DataOffset).CopyTo(bytes, 20);
        BitConverter.GetBytes(DataAddress).CopyTo(bytes, 24);
        BitConverter.GetBytes(DataLength).CopyTo(bytes, 28);
        BitConverter.GetBytes(DataCrc32).CopyTo(bytes, 32);

        BitConverter.GetBytes(NewAppLength).CopyTo(bytes, 36);
        BitConverter.GetBytes(NewAppCrc32).CopyTo(bytes, 40);

        BitConverter.GetBytes(BackupLength).CopyTo(bytes, 44);
        BitConverter.GetBytes(BackupCrc32).CopyTo(bytes, 48);

        byte[] verBytes = Encoding.ASCII.GetBytes(Version);
        Array.Copy(verBytes, 0, bytes, 52, Math.Min(verBytes.Length, 128));

        BitConverter.GetBytes(ThisAddress).CopyTo(bytes, 248);

        // 重新计算Header的CRC32并写入
        ThisCrc32 = CRC.CRC32(bytes[..252]);
        BitConverter.GetBytes(ThisCrc32).CopyTo(bytes, 252);

        return bytes;
    }
}