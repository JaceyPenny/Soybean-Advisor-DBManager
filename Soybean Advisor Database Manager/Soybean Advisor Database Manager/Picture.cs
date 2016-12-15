using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Soybean_Advisor_Database_Manager
{
    [ParseClassName("Picture")]
    public class Picture : ParseObject
    {
        [ParseFieldName("fileName")]
        public string FileName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("itemId")]
        public int ItemId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("pictureId")]
        public int PictureId
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("source")]
        public string Source
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("file")]
        public ParseFile File
        {
            get { return GetProperty<ParseFile>(); }
            set { SetProperty<ParseFile>(value); }
        }

        public Picture()
        {

        }
    }
}
