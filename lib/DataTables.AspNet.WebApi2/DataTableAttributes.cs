using System.Collections.Generic;

namespace DataTables.AspNet.WebApi2
{
    public class DataTableDom
    {
        public string el { get; set; }
        public string content { get; set; }
    }
    
    public class DataTableAttributes
    {
        public bool AutoWidth { get; set; } //bAutoWidth 
        public bool DeferRender { get; set; } //bDeferRender 
        public bool Filter { get; set; } //bFilter 
        public bool Info { get; set; } //bInfo  
        public bool JQueryUI { get; set; } //bJQueryUI  
        public bool LengthChange { get; set; } //bLengthChange  
        //Paginate should be always enabled!
        public bool Paginate { get; set; } //bPaginate  
        public bool ScrollInfinite { get; set; } //bScrollInfinite
        //bServerSide 
        public bool Sort { get; set; } //bSort  
        public bool SortClasses { get; set; } //bSortClasses  
        public bool StateSave { get; set; } //bStateSave 
        public string ScrollX { get; set; } //bStateSave 
        public string ScrollY { get; set; } //sScrollY 
        public List<ColumnDef> ColumnDefs { get; set; }
        public PaginationTypes PaginationType { get; set; } //sPaginationType 
        public bool ScrollCollapse { get; set; }
        public string Dom { get; set; }
        public List<DataTableDom> Doms { get; set; }

        public DataTableAttributes()
        {
            AutoWidth = true;
            Filter = true;
            Info = true;
            LengthChange = true;
            Paginate = true;
            Sort = true;
            SortClasses = true;
            PaginationType = PaginationTypes.FullNumbers;
            Doms = new List<DataTableDom>();
        }

    }
}
