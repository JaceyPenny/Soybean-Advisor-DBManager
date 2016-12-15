using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Data")]
    class Data: ParseObject
    {
        [ParseFieldName("diseaseVersion")]
        public int DiseaseVersion
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("insectVersion")]
        public int InsectVersion
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("deficiencyVersion")]
        public int DeficiencyVersion
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("limerate")]
        public IList<IList<int>> LimeRate
        {
            get { return GetProperty<IList<IList<int>>>(); }
            set { SetProperty<IList<IList<int>>>(value); }
        }

        [ParseFieldName("phosphorus")]
        public IList<IList<int>> Phosphorus
        {
            get { return GetProperty<IList<IList<int>>>(); }
            set { SetProperty<IList<IList<int>>>(value); }
        }

        [ParseFieldName("potassium")]
        public IList<IList<int>> Potassium
        {
            get { return GetProperty<IList<IList<int>>>(); }
            set { SetProperty<IList<IList<int>>>(value); }
        }

        [ParseFieldName("insectThresholds")]
        public IList<IList<int>> InsectThresholds
        {
            get { return GetProperty<IList<IList<int>>>(); }
            set { SetProperty<IList<IList<int>>>(value); }
        }

        public Data()
        {

        }
    }
}
