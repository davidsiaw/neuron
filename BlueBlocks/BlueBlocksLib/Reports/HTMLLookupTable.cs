using System;
using System.Collections.Generic;
using System.Text;
using BlueBlocksLib.Collections;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Reports
{

    public class HTMLLookupTable
    {

        Set<string> items = new Set<string>();

        Dictionary<Pair<string, string>, string> leftTop = new Dictionary<Pair<string, string>, string>();

        public HTMLLookupTable()
        {
        }

        public string this[string a, string b]
        {
            set
            {
                Pair<string, string> p = new Pair<string, string>() { a = a, b = b };
                items.Add(a);
                items.Add(b);
                leftTop[p] = value;
            }
        }

        public string Render()
        {
            string res = "<table class=\"clothed\">";
            string[] itemarray = ArrayUtils.ToArray(items);

            res += "<tr><td>&nbsp;</td>" + string.Join("",
                 ArrayUtils.ConvertAll(itemarray, x => "<td>" + x + "</td>")) + "</tr>";

            //res += "<tr><td>&nbsp;</td>" + string.Join("", 
            //    itemarray.Select(x=> "<td>" + 
            //        string.Join("<br />", 
            //        x.ToCharArray().Select(y=>y.ToString()).ToArray()) + "</td>")
            //        .ToArray()) + "</tr>";

            for (int i = 0; i < itemarray.Length; i++)
            {
                res += "<tr><td>" + itemarray[i] + "</td>";

                for (int j = 0; j < itemarray.Length; j++)
                {
                    res += "<td align=\"center\">";
                    Pair<string, string> p1 = new Pair<string, string>() { a = itemarray[i], b = itemarray[j] };
                    Pair<string, string> p2 = new Pair<string, string>() { a = itemarray[j], b = itemarray[i] };
                    if (leftTop.ContainsKey(p1))
                    {
                        res += leftTop[p1];
                    }
                    else if (leftTop.ContainsKey(p2))
                    {
                        res += leftTop[p2];
                    }
                    else
                    {
                        res += "-";
                    }
                    res += "</td>";

                }
                res += "</tr>";
            }
            return res + "</table>";
        }

    }
}
