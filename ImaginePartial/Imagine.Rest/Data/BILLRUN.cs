//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Imagine.Rest.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BILLRUN
    {
        public decimal BILLRUNID { get; set; }
        public Nullable<decimal> LOBID { get; set; }
        public Nullable<System.DateTime> DATEFROM { get; set; }
        public Nullable<System.DateTime> DATETO { get; set; }
        public Nullable<short> ISCOMPLETED { get; set; }
        public Nullable<short> ISEXPORTED { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public Nullable<short> ISMIDMONTH { get; set; }
        public Nullable<System.DateTime> INBOUNDDATEFROM { get; set; }
        public Nullable<System.DateTime> INBOUNDDATETO { get; set; }
        public string DIRECTION { get; set; }
    }
}
