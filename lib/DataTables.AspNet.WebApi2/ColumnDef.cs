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

        public ColumnDef()
        {
            UseRendered = true;
            Sorting = SortingDirections.Both;
            Sortable = true;
            Visible = true;
        }
    }
}
