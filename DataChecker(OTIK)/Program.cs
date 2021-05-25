using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataChecker_OTIK_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            MainMenu();
            Console.WriteLine();
            Console.ReadKey();
        }

        static void MainMenu()
        {
            bool menuOn = true;
            while(menuOn)
            {
                TextColor(ConsoleColor.White);
                Console.Write("[");
                TextColor(ConsoleColor.Red);
                Console.Write("O");
                TextColor(ConsoleColor.White);
                Console.Write("] - Переглянути файл;\n[");
                TextColor(ConsoleColor.Red);
                Console.Write("C");
                TextColor(ConsoleColor.White);
                Console.Write("] - Закодувати файл циклічним кодером; \n[");
                TextColor(ConsoleColor.Red);
                Console.Write("E");
                TextColor(ConsoleColor.White);
                Console.Write("] - Внести одиничну помилку у файл; \n[");
                TextColor(ConsoleColor.Red);
                Console.Write("D");
                TextColor(ConsoleColor.White);
                Console.Write("] - Декодувати файл;\n");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.O)
                {
                    Console.Clear();
                    int[] bin = FileManager();
                    ShowTable(bin, 0);
                    Console.WriteLine();
                    TextColor(ConsoleColor.White);
                    Console.WriteLine("Натисніть будь-яку клавішу, щоби продовжити.");
                    Console.ReadKey();
                }
                if(key == ConsoleKey.C)
                {
                    Console.Clear();
                    int[] bin = FileManager();
                    int[] coded = Coder_Heming(bin);
                    SaveFile(coded, Encoding.BigEndianUnicode);
                    TextColor(ConsoleColor.White);
                    Console.WriteLine("Натисніть будь-яку клавішу, щоби продовжити.");
                    Console.ReadKey();
                }
                if (key == ConsoleKey.E)
                {
                    Console.Clear();
                    int[] bin = FileManager();
                    ShowTable(bin, 0);
                    int[] death = Death_Bit(bin);
                    ShowTable(death, 0);
                    SaveFile(death, Encoding.BigEndianUnicode);
                    TextColor(ConsoleColor.White);
                    Console.WriteLine("Натисніть будь-яку клавішу, щоби продовжити.");
                    Console.ReadKey();
                }
                if (key == ConsoleKey.D)
                {
                    Console.Clear();
                    int[] bin = FileManager();
                    ShowTable(bin, 0);
                    int[] decoded = Decoder_Heming(bin);
                    ShowTable(decoded, 0);
                    SaveFile(decoded, Encoding.BigEndianUnicode);
                    TextColor(ConsoleColor.White);
                    Console.WriteLine("Натисніть будь-яку клавішу, щоби продовжити.");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                }
                
            }
        }
        static void Action(int[] bin)
        {
            TextColor(ConsoleColor.White);
            Console.Write("[");
            TextColor(ConsoleColor.Red);
            Console.Write("O");
            TextColor(ConsoleColor.White);
            Console.Write("] - Переглянути інший файл; ");
            Console.Write("[");
            TextColor(ConsoleColor.Red);
            Console.Write("S");
            TextColor(ConsoleColor.White);
            Console.Write("] - Зберегти файл;");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.O)
            {
                Console.Clear();
                bin = FileManager();
                ShowTable(bin, 0);
                Console.WriteLine();
                Action(bin);
            }
            if (key == ConsoleKey.S)
            {
                SaveFile(bin, Encoding.BigEndianUnicode);
                Action(bin);
            }
        }

        static void TextColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(16, (char)(0)));
            }
            return sb.ToString();
        }

        /*public static string BinaryToString(int[] bin)
        {
            string data = string.Join("", bin);
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.BigEndianUnicode.GetString(byteList.ToArray());
        }*/
        static string BinaryToString(int[] bin)
                {
                    int arrSize = bin.Length;
                    if ((arrSize % 16) != 0)
                    {
                        arrSize += 16 - arrSize % 16;
                    }
                    int[] test = new int[arrSize];

                    for (int i = 0; i < bin.Length; i++)
                    {
                        test[i] = bin[i];
                    }

                    string outp = "";
                    int tmp = 0;
                    int size_file = test.Length;

                    if ((test.Length % 16) != 0)
                    {
                        size_file += 16 - (test.Length % 16);
                    }
                    int[] file_bin_new = new int[size_file];
                    for (int i = 0; i < test.Length; i++)
                    {
                        file_bin_new[i] = test[i];
                    }
                    for (int i = 0; i < file_bin_new.Length; i += 16)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            tmp |= file_bin_new[i + 15 - j] << ((i + j) % 16);
                        }
                        outp += Convert.ToChar(tmp);
                        tmp = 0;
                    }
                    return outp;
                }
        
        static int[] FileManager()
        {
            string[] allfiles = Directory.GetFiles(@"ForFiles\", "*.txt", SearchOption.AllDirectories);
            bool wait_For_File = true;
            int pos = 65469873;
            string path = "";;
            while (wait_For_File)
            {
                try
                {
                    Console.WriteLine("Список файлів у папці: ");
                    for (int i = 0; i < allfiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}) {allfiles[i]}");
                    }
                    Console.Write("Введіть порядкойвий номер файлу, щоби відкрити: ");
                    pos = Convert.ToInt32(Console.ReadLine()) - 1;
                    path = allfiles[pos];
                    wait_For_File = false;
                }
                catch
                {
                    Console.WriteLine("Ведено не вірний номер файлу!" +
                        "\nНатисність будь-яку клавішу для продовження.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            wait_For_File = true;

            Console.Clear();
            StreamReader sr = new StreamReader(path, Encoding.BigEndianUnicode);
            string reader = sr.ReadToEnd();
            char[] binaryChar = StringToBinary(reader).ToCharArray();

            int[] binary = new int[binaryChar.Length];

            for (int i = 0; i < binary.Length; i++)
            {
                binary[i] = 0;
                if (binaryChar[i] == '1') binary[i] = 1;
            }
            Console.WriteLine($"Файл: {path}");
            return binary;
        }

        static void SaveFile(int[] bin, Encoding encoding)
        {
            string data = BinaryToString(bin);
            int file_stat = 3;
            TextColor(ConsoleColor.White);
            Console.Write("\nЗберегти файл(");
            Console.Write("[");
            TextColor(ConsoleColor.Red);
            Console.Write("Y");
            TextColor(ConsoleColor.White);
            Console.Write("] - Так / [");
            TextColor(ConsoleColor.Red);
            Console.Write("N");
            TextColor(ConsoleColor.White);
            Console.Write("] - Ні) ");
            bool wait = true;
            while(wait)
            {
                ConsoleKey kk = Console.ReadKey().Key;
                if (kk == ConsoleKey.Y)
                {
                    file_stat = 1;
                    wait = false;
                }
                if(kk == ConsoleKey.N)
                {
                    file_stat = 2;
                    wait = false;
                }
            }
            string[] allfiles = Directory.GetFiles(@"ForFiles\", "*.txt", SearchOption.AllDirectories);
            while (file_stat == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nСписок файлів у папці: ");
                for (int i = 0; i < allfiles.Length; i++)
                {
                    Console.WriteLine($"{i + 1}) {allfiles[i]}");
                }
                Console.Write("\nВведіть ім'я файлу: ");

                string path = @"ForFiles\" + Console.ReadLine();
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, data, encoding);
                }
                try
                {
                    File.WriteAllText(path, data, encoding);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nФайл збережено!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    file_stat = 3;
                    allfiles = Directory.GetFiles(@"ForFiles\", "*.txt", SearchOption.AllDirectories);
                    Console.WriteLine("Список файлів у папці: ");
                    for (int i = 0; i < allfiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}) {allfiles[i]}");
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Файл використовується іншим процесом, спробуйте інше ім'я!\n");
                }
            }
        }

        static void ShowTable(int[] binary, int num)
        {
            int rows = 0;
            Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────┬────────╕");
            Console.WriteLine("    │    0   │    1   │    2   │   3    │   4    │   5    │   6    │    7   │    8   │    9   │   10   │   11   │   12   │   13   │   14   │   15   │   16   │   17   │   18   │   19   │   20   │   21   │   22   │   23   │   24   │   25   │   26   │   27   │   28   │");
            Console.Write("────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┤");
            for (int i = 0; i < binary.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                if (i % 232 == 0)
                {

                    if (rows % 8 == 0)
                    {
                        if (rows != 0)
                        {
                            Console.Write("\n────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┼────────┤");
                        }
                    }
                    else
                    {
                        Console.Write("│");
                    }
                    if (rows.ToString().Length == 1)
                    {
                        if (i != 0)
                        {
                            Console.Write($"\n{rows}   │");
                        }
                        else
                        {
                            Console.Write($"\n{rows}   │");
                        }
                        
                    }
                    if (rows.ToString().Length == 2)
                    {
                        Console.Write($"\n{rows}  │");
                    }
                    if (rows.ToString().Length == 3)
                    {
                        Console.Write($"\n{rows} │");
                    }
                    if (rows.ToString().Length == 4)
                    {
                        Console.Write($"\n{rows} │");
                    }

                    rows++;
                }
                
                if (i % 8 == 0 && i % 232!= 0)
                {
                    Console.Write("│");
                }
                if (binary[i] == (1))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("♦");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("•");
                }
            }
        }

        static int[] Coder_Heming(int[] bin)
        {
            int[] coded_bin = new int[bin.Length * 7 / 4];
            int[] data = new int[7];
            int[] polinom = { 1, 1, 0, 1 };
            int[] end = new int[3];
            int count_1 = 0;
            int Count = 0;
            for (int i = 0; i < bin.Length; i++)
            {
                if ((i % 4) == 3)
                {
                    data[0] = bin[i - 3];
                    data[1] = bin[i - 2];
                    data[2] = bin[i - 1];
                    data[3] = bin[i - 0];
                    data[4] = 0;
                    data[5] = 0;
                    data[6] = 0;
                    Division_Polinom(data, polinom, out end, out count_1);
                    coded_bin[Count + 0] = data[0];
                    coded_bin[Count + 1] = data[1];
                    coded_bin[Count + 2] = data[2];
                    coded_bin[Count + 3] = data[3];
                    coded_bin[Count + 4] = end[0];
                    coded_bin[Count + 5] = end[1];
                    coded_bin[Count + 6] = end[2];
                    Count += 7;
                }
            }
            ShowTable(coded_bin, 1);
            return coded_bin;
        }

        static int[] Decoder_Heming(int[] bin)
        {
            int arr_size = bin.Length - (bin.Length % 28);
            int[] decoded_bin = new int[arr_size * 4 / 7];
            int[] data = new int[7];
            int[] polinom = { 1, 1, 0, 1 };
            int[] end = new int[3];
            int[] data_out = new int[7];
            int count_1 = 0;
            int cicle_count = 0;
            int Count = 0;
            for (int i = 0; i < arr_size; i++)
            {
                if ((i % 7) == 6)
                {
                    data[0] = bin[i - 6];
                    data[1] = bin[i - 5];
                    data[2] = bin[i - 4];
                    data[3] = bin[i - 3];
                    data[4] = bin[i - 2];
                    data[5] = bin[i - 1];
                    data[6] = bin[i - 0];
                    Division_Polinom(data, polinom, out end, out count_1);
                    Revers_Register(data, 0, 'r', out data_out);
                    while (count_1 > 1)
                    {
                        cicle_count++;
                        Revers_Register(data, cicle_count, 'l', out data_out);
                        Division_Polinom(data_out, polinom, out end, out count_1);
                    }
                    data_out[4] ^= end[0];
                    data_out[5] ^= end[1];
                    data_out[6] ^= end[2];
                    Revers_Register(data_out, cicle_count, 'r', out data);
                    cicle_count = 0;
                    decoded_bin[Count + 0] = data[0];
                    decoded_bin[Count + 1] = data[1];
                    decoded_bin[Count + 2] = data[2];
                    decoded_bin[Count + 3] = data[3];
                    Count += 4;
                }
            }
            return decoded_bin;
        }

        private static void Division_Polinom(int[] data, int[] polinom, out int[] end, out int w_count)
        {
            int[] coded_data = new int[7];
            for (int i = 0; i < 7; i++)
            {
                coded_data[i] = data[i];
            }
            end = new int[3];
            for (int j = 0; j <= 3; j++)
            {
                if (coded_data[j] == 1)
                {
                    coded_data[j] ^= polinom[0];
                    coded_data[j + 1] ^= polinom[1];
                    coded_data[j + 2] ^= polinom[2];
                    coded_data[j + 3] ^= polinom[3];
                }
            }
            end[0] = coded_data[4];
            end[1] = coded_data[5];
            end[2] = coded_data[6];
            w_count = coded_data[4] + coded_data[5] + coded_data[6];
        }

        private static void Revers_Register(int[] data_in, int n_shift, char revers, out int[] data_out)
        {
            int[] data_k = new int[7];
            for (int i = 0; i < 7; i++)
            {
                data_k[i] = data_in[i];
            }
            data_out = new int[7];
            for (int f = 0; f < 7; f++)
            {
                if (revers == 'r')
                {
                    data_out[f] = data_k[(f + n_shift) % 7];
                }
                else if (revers == 'l')
                {
                    data_out[(f + n_shift) % 7] = data_k[f];
                }

            }
        }
        static int[] Death_Bit(int[] bin)
        {
            Console.Write("\nВедіть коодинати для спотворення біту(Вертикаль/горизонталь): ");
            string read_answ = Console.ReadLine();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] tmp = new string[2];
            tmp = read_answ.Split(delimiterChars);
            int[] VerticalHorisontal = new int[2];
            VerticalHorisontal[0] = Convert.ToInt32(tmp[0]);
            VerticalHorisontal[1] = Convert.ToInt32(tmp[1]);
            try
            {
                if (bin.Length < 232)
                {
                    var twoDArray = bin;
                    twoDArray[VerticalHorisontal[1]] = 1 - twoDArray[VerticalHorisontal[1]];
                }
                else
                {
                    var twoDArray = Make2DArray(bin, (bin.Length / 232), 232);

                    twoDArray[VerticalHorisontal[0], VerticalHorisontal[1]] = 1 - twoDArray[VerticalHorisontal[0], VerticalHorisontal[1]];
                    int k = 0;
                    for (int i = 0; i < bin.Length / 232; i++)
                    {
                        for (int j = 0; j < 232; j++)
                        {
                            bin[k++] = twoDArray[i, j];
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Введено неправильні координати біту!");
            }
            return bin;
        }

        static T[,] Make2DArray<T>(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }
    }
}
