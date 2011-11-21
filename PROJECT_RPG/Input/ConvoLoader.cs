using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class ConvoLoader
    {
        public static String[] LoadConvo(String filename)
        {
            StreamReader reader = new StreamReader(filename);
            int width = Int32.Parse(reader.ReadLine());
            String[] convo = new String[width];
            String line = reader.ReadLine();
            int index = 0;
            while (line != null && index < width)
            {
                convo[index++] = line;
                line = reader.ReadLine();
            }
            return convo;
        }
    }
}
