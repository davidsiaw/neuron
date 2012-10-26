using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using BlueBlocksLib.SetUtils;
using System.Runtime.InteropServices;
using System.Collections;

namespace BlueBlocksLib.Reports {

	[AttributeUsage(AttributeTargets.Field)]
	public class HeaderNameAttribute : Attribute {
		public string Name { get; private set; }
		public HeaderNameAttribute(string name) {
			Name = name;
		}
	}

    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnSizeAttribute : Attribute
    {
        public int Size { get; private set; }
        public ColumnSizeAttribute(int size)
        {
            Size = size;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MultiColumn : Attribute
    {
        public int Size { get; private set; }
        public MultiColumn(int size)
        {
            Size = size;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnClassAttribute : Attribute
    {
        public string Class { get; private set; }
        public ColumnClassAttribute(string cls)
        {
            Class = cls;
        }
    }

    public enum FilterType
    {
        Numeric,
        ComboBox,
        PrefixFilter,
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnFilterTypeAttribute : Attribute
    {
        public FilterType FilterType { get; private set; }
        public ColumnFilterTypeAttribute(FilterType type)
        {
            FilterType = type;
        }
    }


	public class HTMLTable<TData> {

		TData[] m_data;
        int itemsPerPage;

        public HTMLTable(TData[] data, int itemsPerPage = -1)
        {
			m_data = data;
            this.itemsPerPage = itemsPerPage;
		}

		public string ClassName = null;

        public string Render(Func<string, string> translationDelegate)
        {
            string[] content = Array.ConvertAll(m_data, x => RenderData(x));

			string[] rows = ArrayUtils.ConvertAll(content, x => "<tr>" + x + "</tr>");

            tableid = tablecount++;

            if (itemsPerPage > 0)
            {
                ClassName += " table-page-number:t" + tableid + "page table-page-count:t" + tableid + "pages table-autopage:" + itemsPerPage;
            }

			return "<table " + (ClassName == null ? "" : ("class=\"" + ClassName + "\"")) + ">" +
				"<thead><tr>" +
				RenderHeader(translationDelegate) +
                "</tr>" + (itemsPerPage > 0 ? RenderPageFlipper() : "") + "</thead>" +
				string.Join("", rows) +
				"</table>";
        }

        static int tablecount = 0;

        int tableid;

        private string RenderPageFlipper()
        {
            tablecount++;
            return @"
<tr>
<td colspan=""" + m_fieldsInOrder.Length + @""" style=""text-align:center""><span style=""cursor:pointer"" onClick=""Table.pageJump(this,-10)"">&lt;&lt;</span> | <span style=""cursor:pointer"" onClick=""Table.pageJump(this,-1)"">&lt;</span> | Page <span id=""t" + tableid + @"page""></span> of <span id=""t" + tableid + @"pages""></span> Pages | <span style=""cursor:pointer"" onClick=""Table.pageJump(this,1)"">&gt;</span> | <span style=""cursor:pointer"" onClick=""Table.pageJump(this,10)"">&gt;&gt;</span></td>
</tr>
";
        }

		public string Render() {
            return Render(x => x);
		}

		FieldInfo[] m_fieldsInOrder = null;

		string[] ConvertFields(Func<string, FieldInfo> converter) {

			if (m_fieldsInOrder == null) {
				m_fieldsInOrder = typeof(TData).GetFields();
				m_fieldsInOrder = ArrayUtils.FindAll(m_fieldsInOrder, x =>
					x.GetCustomAttributes(typeof(HeaderNameAttribute), false).Length != 0);

				Array.Sort(m_fieldsInOrder, (x, y) =>
					Marshal.OffsetOf(typeof(TData), x.Name).ToInt64().CompareTo(
					Marshal.OffsetOf(typeof(TData), y.Name).ToInt64()));
			}

			return ArrayUtils.ConvertAll(m_fieldsInOrder, x => converter(x));
		}

        string RenderHeader(Func<string, string> translationDelegate)
        {
            bool filterExists = false;
			string[] headerNames = ConvertFields(x => {
				HeaderNameAttribute attr = (HeaderNameAttribute)
					x.GetCustomAttributes(typeof(HeaderNameAttribute), false)[0];

                MultiColumn[] multicolumn = (MultiColumn[])
                    x.GetCustomAttributes(typeof(MultiColumn), false);

                ColumnFilterTypeAttribute[] colfilter = (ColumnFilterTypeAttribute[])
                    x.GetCustomAttributes(typeof(ColumnFilterTypeAttribute), false);
                filterExists |= colfilter.Length != 0;

                if (multicolumn.Length > 0)
                {
                    string header = "";
                    for (int i = 0; i < multicolumn[0].Size; i++)
                    {

                        header += MakeColumnHeader(x, attr, translationDelegate);
                    }

                    return header;
                }

                return MakeColumnHeader(x, attr, translationDelegate);
			});

            return string.Join("\r\n", headerNames) + (filterExists ? "</tr><tr>" + string.Join("\r\n", ConvertFields(x => MakeColumnFilter(x))) : "");
		}

        static int columnNum = 0;

        private static string MakeColumnFilter(FieldInfo x)
        {

            ColumnFilterTypeAttribute[] colfilter = (ColumnFilterTypeAttribute[])
                x.GetCustomAttributes(typeof(ColumnFilterTypeAttribute), false);

            if (colfilter.Length != 0)
            {
                ColumnFilterTypeAttribute filter = colfilter[0];
                switch (filter.FilterType)
                {
                    case FilterType.ComboBox:
                        return "<th class=\"table-filterable\"></th>";
                    case FilterType.PrefixFilter:
                        return "<th><input name=\"filter" + columnNum++ + "\" size=\"8\" onkeyup=\"Table.filter(this,this)\"></th>";
                    case FilterType.Numeric:
                        int col = columnNum++;
                        return @"
<th class="""">
<select id=""column" + col + @""">
<option value="""">All</option>
<option value=""<"">below</option>
<option value="">"">over</option>
<input id=""column" + col + @"filter"" name=""filter" + col + @""" size=""4"" onkeyup=""this.customFilter=function(x){ if (dojo.byId('column" + col + @"').value == '>') { return parseFloat(x.replace(/,/g,'')) >= parseFloat(dojo.byId('column" + col + @"filter').value); } else if (dojo.byId('column" + col + "').value == '<') { return parseFloat(x.replace(/,/g,'')) <= parseFloat(dojo.byId('column" + col + @"filter').value); } else { return true; } ;}; Table.filter(this,this)"" >
</th>";
                }
            }

            return "<th></th>";
        }

        private static string MakeColumnHeader(FieldInfo x, HeaderNameAttribute attr, Func<string,string> translationDelegate)
        {
            ColumnSizeAttribute[] colsize = (ColumnSizeAttribute[])
                x.GetCustomAttributes(typeof(ColumnSizeAttribute), false);

            ColumnClassAttribute[] colclass = (ColumnClassAttribute[])
                x.GetCustomAttributes(typeof(ColumnClassAttribute), false);
            
            return "<th" + InsertColClass(colclass) + InsertColSize(colsize) +">" + translationDelegate(attr.Name) + "</th>";
        }

        private static string InsertColClass(ColumnClassAttribute[] colclass)
        {
            return (colclass.Length != 0 ? " class=\"" + colclass[0].Class + "\"" : "");
        }

        private static string InsertColSize(ColumnSizeAttribute[] colsize)
        {
            return (colsize.Length != 0 ? " width=\"" + colsize[0].Size + "px\"" : "");
        }

		string RenderData(TData row) {
			return string.Join("", ConvertFields(x => {
                if (x.FieldType.IsArray)
                {
                    return string.Join("", ArrayUtils.ConvertAll((object[])x.GetValue(row),
                        item => "<td>" + item.ToString() + "</td>"));
                }
                return "<td>" + x.GetValue(row) + "</td>";
            }
                ));
		}
	}
}
