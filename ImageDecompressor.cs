using System;
using System.IO;

public static class ImageDecompressor
{
    public static void UncompressFile(string inputPath, string filename, string outputPath)
    {
        string sWidth, sHeight;

        using FileStream fs = File.OpenRead(inputPath + filename);

        ushort width = ReadUShort(fs);
        ushort height = ReadUShort(fs);
        Console.WriteLine("W: " + width + " H: " + height);
        fs.Position = 20;

        int inputSize = (int)(fs.Length - 20) / 2;
        ushort[] input = new ushort[inputSize];
        byte[] buffer = new byte[2];
        for (int i = 0; i < inputSize; i++)
        {
            fs.Read(buffer, 0, 2);
            input[i] = BitConverter.ToUInt16(buffer, 0);
        }

        int outputSize = width * height;
        ushort[] output = new ushort[outputSize];


        try
        {
            // i = input position, o = output position
            for (int i = 0, o = 0; i < input.Length;)
            {
                ushort value = input[i];
                i++;

                if (value < 0x100)
                {
                    int count = value + 1;
                    
                    // direct copy
                    for (int j = 0; j < count; j++)
                        output[o + j] = input[i + j];

                    i += value + 1;
                    o += value + 1;
                }
                else
                {
                    int index = value >> 8;
                    int count = (value & 0xFF) + 1;

                    // invalid index, read the next uint16
                    if (index == 0xFF)
                    {
                        index = input[i];
                        i++;
                    }

                    // "big count" flag, read the next uint32
                    if (count == 0x100)
                    {
                        count = input[i] | (input[i + 1] << 16);
                        i += 2;
                    }

                    // back reference using a relative index
                    for (int j = 0; j < count; j++, o++)
                        output[o] = output[o - index];

                }
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e);
        }

        if (width < 100) { sWidth = "0" + width.ToString(); } else { sWidth = width.ToString(); }
        if (height < 100) { sHeight = "0" + width.ToString(); } else { sHeight = height.ToString(); }

        outputPath = outputPath + filename;
        outputPath = outputPath.Substring(0, outputPath.Length - 4) + "_" + sWidth + "x" + sHeight + ".raw";
        Console.WriteLine(outputPath);

        // Create a new FileStream to write the output
        using (FileStream outputStream = File.Create(outputPath))
        {
            // Write the ushort values from the output array to the new file
            for (int i = 0; i < output.Length; i++)
            {
                byte[] valueBytes = BitConverter.GetBytes(output[i]);
                outputStream.Write(valueBytes, 0, valueBytes.Length);
            }
        }
    }

    // Method to read a ushort from a FileStream
    static ushort ReadUShort(FileStream fs)
    {
        byte[] buffer = new byte[2];
        fs.Read(buffer, 0, 2);
        return BitConverter.ToUInt16(buffer, 0);
    }
}