using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataTables.AspNet.WebApi2
{
    public class ColumnDef
    {
        //public int[] DataSort { get; set; } // DataTables :: aoColumnDefs :: aDataSort
        public SortingDirections Sorting { get; set; } // DataTables :: aoColumnDefs :: asSorting
        //public bool Searchable { get; set; } // DataTables :: aoColumnDefs :: bSearchable 
        public bool Sortable { get; set; } // DataTables :: aoColumnDefs :: bSortable
        public bool UseRendered { get; set; } // DataTables :: aoColumnDefs :: bUseRendered
        public bool Visible { get; set; } // DataTables :: aoColumnDefs :: bVisible 
        public string FnCreatedCell { get; set; } // DataTables :: aoColumnDefs :: fnCreatedCell
        public string FnRender { get; set; } // DataTables :: aoColumnDefs :: fnRender
        //public int DataSort { get; set; } // DataTables :: aoColumnDefs :: iDataSort 
        public string DataProp { get; set; } // DataTables :: aoColumnDefs :: mDataProp
        public string CssClass { get; set; } // DataTables :: aoColumnDefs :: sClass
        public string DefaultContent { get; set; } // DataTables :: aoColumnDefs :: sDefaultContent
        public string Name { get; set; } // DataTables :: aoColumnDefs :: sName
        //public DataTypes?? SortDataType { get; set; } // DataTables :: aoColumnDefs :: sSortDataType
        public string Title { get; set; } // DataTables :: aoColumnDefs :: sTitle
        //public DataTypes?? Type { get; set; } // DataTables :: aoColumnDefs :: sType
        public string Width { get; set; } // DataTables :: aoColumnDefs :: sWidth
        public int Position { get; set; }

        public ColumnDef()
        {
            UseRendered = true;
            Sorting = SortingDirections.Both;
            Sortable = true;
            Visible = true;
        }

        public static List<ColumnDef> From<T>() where T : class, new() =>
            typeof(T).GetProperties().Where(x => x.GetCustomAttributes(typeof(ColumnDefAttribute), false).Any())
            .Select(x => new { c = x.GetCustomAttributes(typeof(ColumnDefAttribute), false).First() as ColumnDefAttribute, p = x })
            .OrderBy(x => x.c.Position)
            .Select(x => x.c.ToColumn(x.p)).ToList();
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ColumnDefAttribute : Attribute
    {
        public SortingDirections Sorting { get; set; }
        public bool Sortable { get; set; }
        public bool UseRendered { get; set; }
        public bool Visible { get; set; }
        public int Position { get; private set; }
        public string FnCreatedCell { get; set; }
        public string FnRender { get; set; }
        public string DataProp { get; set; }
        public string CssClass { get; set; }
        public string DefaultContent { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Width { get; set; }

        public ColumnDefAttribute(int pos)
        {
            UseRendered = true;
            Sorting = SortingDirections.Both;
            Sortable = true;
            Visible = true;
            Position = pos;
        }

        public ColumnDef ToColumn(PropertyInfo propertyInfo)
        {
            return new ColumnDef
            {
                CssClass = CssClass,
                DataProp = DataProp ?? propertyInfo.Name,
                DefaultContent = DefaultContent,
                FnCreatedCell = FnCreatedCell,
                FnRender = FnRender,
                Name = Name ?? propertyInfo.Name,
                Sortable = Sortable,
                Sorting = Sorting,
                Title = Title ?? propertyInfo.Name,
                UseRendered = UseRendered,
                Visible = Visible,
                Width = Width,
                Position = Position
            };
        }
    }
}
