using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fx.IO;

namespace COMunicator
{
    public class History
    {
        string[] items;
        int itemsCount;
        int selectedIndex;
        string fileName;

        public History()
        {
            items = new string[0];
        }

        public History(string fileName, int historyCount = 100)
        {
            this.fileName = Paths.GetFullPath(fileName);
            CreateHistory(this.fileName, historyCount);
        }


        public void Load(string fileName, int historyCount = 100)
        {
            this.fileName = fileName;
            CreateHistory(fileName, historyCount);
        }

        public void Save()
        {
            Files.Save(Paths.GetFullPath(fileName), items);
        }

        public string Get()
        {
            if (selectedIndex > -1 && selectedIndex < items.Length)
                return items[selectedIndex];
            else
                return "";
        }

        public void Add(string item)
        {
            // ----- find command in history -----
            int pos = -1;
            for (int i = 1; i < items.Length; i++ )
            {
                if (item == items[i]) pos = i;
            }
            if (pos != 1)
            {
                if (pos == -1)
                {
                    if (itemsCount < items.Length) itemsCount++;
                    // shift buffer and write new item
                    for (int i = items.Length - 1; i > 1; i--)
                    {
                        items[i] = items[i - 1];
                    }
                }
                else
                {
                    // shift buffer and write new item
                    for (int i = pos; i > 1; i--)
                    {
                        items[i] = items[i - 1];
                    }
                }
                
                items[1] = item;
            }



                /*if (item != items[1])       // if new command
                {
                    if (itemsCount < items.Length) itemsCount++;
                    // shift buffer and write new item
                    for (int i = items.Length - 1; i > 1; i--)
                    {
                        items[i] = items[i - 1];
                    }
                    items[1] = item;
                }*/
            selectedIndex = 0;
        }

        public void SetTemporary(string temp)
        {
            items[0] = temp;
        }

        public string GetNext()
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = 0;
            return items[selectedIndex];
        }

        public string GetPrevious(string temporary)
        {
            if (selectedIndex == 0)
            {
                items[0] = temporary;
            }
            if ((items[selectedIndex + 1] != "") && (items[selectedIndex + 1] != null))
            {
                selectedIndex++;
                if (selectedIndex > 100) selectedIndex = 100;
                return items[selectedIndex];
            }
            return null;
        }

        public string[] GetList()
        {
            string[] res = new string[itemsCount];
            for (int i = 0; i < res.Length; i++) res[i] = items[i];
            return res;
        }

        private void CreateHistory(string fileName, int historyCount = 100)
        {
            items = new string[historyCount+1];
            selectedIndex = 0;


            string[] CntItem = Files.ReadLines(fileName);
            int length = CntItem.Length;
            if (length > (historyCount + 1)) length = historyCount + 1;
            for (int i = 0; i < length; i++)
            {
                if (i >= items.Length) break;
                items[i] = CntItem[i];
            }
            if (length == 0)
            {
                items[0] = "";
                length++;
            }
            itemsCount = length;
        }

        


    }
}
