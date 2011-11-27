using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class ConvoLoader
    {
        public static String[][][] LoadConvo(String filename)
        {
            String[][][] returnable = new String[2][][];
            StreamReader reader = new StreamReader(filename);
            int width = Int32.Parse(reader.ReadLine());
            char[] delims = { ':' };
            String[] needConvoOptTokens = reader.ReadLine().Substring(0).Split(delims);
            String[][] convo = new String[width][];
            for (int i = 0; i < width; i++)
            {
                convo[i] = new string[2];
            }
            String line = reader.ReadLine();
            int index = 0;
            while (line != null && index < width)
            {
                String[] tokens = line.Substring(0).Split(delims);
                convo[index][0] = tokens[0];
                convo[index][1] = tokens[1];
                index++;
                line = reader.ReadLine();
            }
            reader.Close();
            if (needConvoOptTokens[0] == "true")
            {
                returnable[1] = LoadConvoOptions(needConvoOptTokens[1]);
            }
            else
            {
                // do nothing
            }
            returnable[0] = convo;
            return returnable;
        }

        public static String[][] LoadConvoOptions(String filename)
        {
            StreamReader reader = new StreamReader(filename);
            int width = Int32.Parse(reader.ReadLine()) + 1;
            char[] delims = { ':', '/' };
            String[][] convo_options = new String[width][];
            for (int i = 0; i < width; i++)
            {
                convo_options[i] = new string[3];
            }
            String line = reader.ReadLine();
            int index = 0;
            while (line != null && index < width)
            {
                String[] tokens = line.Substring(0).Split(delims);
                convo_options[index][0] = tokens[0];
                convo_options[index][1] = tokens[1];
                convo_options[index][2] = tokens[2];
                index++;
                line = reader.ReadLine();
            }
            return convo_options;
        }
    }
}
