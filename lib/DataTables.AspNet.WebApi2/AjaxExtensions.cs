using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace DataTables.AspNet.WebApi2
{
    public static class AjaxExtensions
    {
        public static MvcHtmlString DataTable(this AjaxHelper ajaxHelper,
            String tableName,
            AjaxOptions ajaxOptions,
            DataTableAttributes dataTableParams,
            UrlHelper url,
            bool isAjax)
        {
            return MvcHtmlString.Create(string.Format("<script type='text/javascript'>\n{0}\n</script>", GenerateOTableScript(tableName, dataTableParams, ajaxOptions, url, isAjax)));
        }

        public static string ToLowerString(this bool boolean)
        {
            return boolean.ToString().ToLower();
        }

        private static string GenerateOTableScript(String tableName, DataTableAttributes dataTableParams, AjaxOptions ajaxOptions, UrlHelper url, bool isAjax)
        {
            return $@"
    var draws = {Newtonsoft.Json.JsonConvert.SerializeObject(dataTableParams.Doms.ToArray())};
    {(!isAjax ? "window.addEventListener('DOMContentLoaded', function () {" : "$(function () {")}
        window.oTable = $('#{tableName}').dataTable({{{(!string.IsNullOrEmpty(dataTableParams.Dom)?$@"
            dom: '{dataTableParams.Dom}',":"")}{(dataTableParams.Doms.Any() ? $@"
            drawCallback: drawCallback," : "")}
            pageLength: {dataTableParams.PageLength},
            aoColumnDefs: {GenerateColumnDefsForOTable(dataTableParams.ColumnDefs)},
            bAutoWidth: {dataTableParams.AutoWidth.ToLowerString()},
            bDeferRender: {dataTableParams.DeferRender.ToLowerString()},
            bFilter: {dataTableParams.Filter.ToLowerString()},
            bInfo: {dataTableParams.Info.ToLowerString()},
            bLengthChange: {dataTableParams.LengthChange.ToLowerString()},
            bPaginate: {dataTableParams.Paginate.ToLowerString()},
            bProcessing: true,
            bScrollInfinite: {dataTableParams.ScrollInfinite.ToLowerString()},
            bScrollCollapse: {dataTableParams.ScrollCollapse.ToLowerString()},
            bServerSide: true,
            bSort: {dataTableParams.Sort.ToLowerString()},
            bSortClasses: {dataTableParams.SortClasses.ToLowerString()},
            bStateSave: {dataTableParams.StateSave.ToLowerString()},{(!string.IsNullOrEmpty(dataTableParams.ScrollX) ? $@"
            sScrollX: {'"' + dataTableParams.ScrollX + '"'}," : "")}{(!string.IsNullOrEmpty(dataTableParams.ScrollY) ? $@"
            sScrollY: {'"' + dataTableParams.ScrollY + '"'}," : "")}
            sPaginationType: {'"' + GetDescription(dataTableParams.PaginationType) + '"'},
            sAjaxSource: {('"' + ajaxOptions.Url + '"')},
            sServerMethod: {'"' + ajaxOptions.HttpMethod + '"'},
            language: {{
                sEmptyTable: '<center>Nenhum registro encontrado</center>',
                sInfo: 'Exibindo de _START_ a _END_ de _TOTAL_ registro(s)',
                sInfoEmpty: '',
                sInfoFiltered: '',
                sInfoPostFix: '',
                sInfoThousands: '.',
                sLengthMenu: '_MENU_ resultados por página',
                sLoadingRecords: 'Carregando...',
                sProcessing: '<img src={'"' + url.Content("~/../img/ajax-loader-2.gif") + '"'} />',
                sZeroRecords: '<center>Nenhum registro encontrado</center>',
                sSearch: 'Pesquisar ',
                oPaginate: {{
                    sNext: 'Próximo',
                    sPrevious: 'Anterior',
                    sFirst: 'Primeiro',
                    sLast: 'Último'
                }},
                oAria: {{
                    sSortAscending: ': Ordenar colunas de forma ascendente',
                    sSortDescending: ': Ordenar colunas de forma descendente'
                }},
            }},
        }});
    }});
";
        }

        private static TagBuilder TableTagBuilder(string tableId, string tableName, string updateTargetId)
        {
            if (!string.IsNullOrWhiteSpace(updateTargetId))
                return new TagBuilder(string.Empty);

            TagBuilder containerDiv = new TagBuilder("div");

            return containerDiv;

        }

        private static MvcHtmlString GenerateColumnDefsForOTable(IEnumerable<ColumnDef> columnDefs)
        {
            if (columnDefs.Count() == 0)
                return MvcHtmlString.Create(string.Empty);

            string result = "[ \n";
            for (int colIndex = 0; colIndex < columnDefs.Count(); colIndex++)
            {
                ColumnDef columnDef = columnDefs.ElementAt(colIndex);
                result += ColumnDefToString(columnDef, colIndex);
                if (colIndex != columnDefs.Count() - 1)
                    result += ", \n";
            }
            result += " ]";

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// Generates a formatted string of ColumnDef for oTable object of DataTables.
        /// This method is used by MVC.Ajax.DataTables.GenerateColumnsForOTable(IEnumerable<ColumnDef> columnDefs).
        /// </summary>
        /// <param name="columnDef">
        /// An instance of ColumnDef class.
        /// </param>
        /// <param name="targets">
        ///The aTargets property is an array to target one of many columns and each element in it can be:
        ///  - a string - class name will be matched on the TH for the column
        ///  - 0 or a positive integer - column index counting from the left
        ///  - a negative integer - column index counting from the right
        ///  - the string "_all" - all columns (i.e. assign a default)
        /// But for now, i just accept an int for "targets" which represent aTargets.
        /// (TODO: int targets --» object[] targets)
        /// </param>
        /// <returns></returns>
        private static string ColumnDefToString(ColumnDef columnDef, int targets)
        {
            string result = "\t\t{\n";
            if (columnDef.Sorting != SortingDirections.Both)
                result += string.Format("\t\t\"aDataSort\": [ \"{0}\" ], \n", GetDescription(columnDef.Sorting));
            if (!columnDef.Sortable) //bSortable default is true
                result += string.Format("\t\t\"bSortable\": {0}, \n", columnDef.Sortable.ToString().ToLower());
            if (!columnDef.UseRendered) //bUseRendered default is true
                result += string.Format("\t\t\"bUseRendered\": {0}, \n", columnDef.UseRendered.ToString().ToLower());
            if (!columnDef.Visible) //bVisible default is true 
                result += string.Format("\t\t\"bVisible\": : {0}, \n", columnDef.Visible);
            if (!string.IsNullOrEmpty(columnDef.FnCreatedCell))
                result += string.Format("\t\t\"fnCreatedCell\": {0}, \n", columnDef.FnCreatedCell);
            if (!string.IsNullOrEmpty(columnDef.FnRender))
                result += string.Format("\t\t\"render\": {0}, \n", columnDef.FnRender);
            if (!string.IsNullOrEmpty(columnDef.CssClass))
                result += string.Format("\t\t\"sClass\": \"{0}\", \n", columnDef.CssClass);
            if (!string.IsNullOrEmpty(columnDef.DefaultContent))
                result += string.Format("\t\t\"sDefaultContent\": {0}, \n", columnDef.DefaultContent);
            if (!string.IsNullOrEmpty(columnDef.DataProp))
                result += string.Format("\t\t\"mDataProp\": \"{0}\", \n", columnDef.DataProp);
            if (!string.IsNullOrEmpty(columnDef.Name))
                result += string.Format("\t\t\"sName\": \"{0}\", \n", columnDef.Name);
            if (!string.IsNullOrEmpty(columnDef.Title))
                result += string.Format("\t\t\"sTitle\": \"{0}\", \n", columnDef.Title);
            if (!string.IsNullOrEmpty(columnDef.Width))
                result += string.Format("\t\t\"sWidth\": \"{0}\", \n", columnDef.Width);
            result += string.Format("\t\t\"aTargets\": [ {0} ] }}", targets);
            return result;
        }

        private static string ResolveTableId(string tableName, string updateTargetId)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                if (string.IsNullOrWhiteSpace(updateTargetId))
                    return GenerateTableId();
                else
                    return updateTargetId;
            else
                return tableName;
        }

        private static string GenerateTableId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return string.Format("table{0}", BitConverter.ToInt64(buffer, 0));
        }

        private static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
    }
}
