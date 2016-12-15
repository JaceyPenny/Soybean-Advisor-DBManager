using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Deficiency")]
    class Deficiency: ParseObject, Pictureable
    {
        [ParseFieldName("deficiencyId")]
        public int DeficiencyId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("description")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("pictureId")]
        public int PictureId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        public Deficiency()
        {

        }

        int Pictureable.GetPictureID()
        {
            return PictureId;
        }
    }
}
