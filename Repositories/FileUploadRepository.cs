using ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LadowebservisMVC.lib.Repositories
{
    public class FileUploadRepository
    {
        // Other methods...

        public string SetFileDescription(string category, string fileName, string fileDescription)
        {
            try
            {
                FileDescriptionRepository rep = new FileDescriptionRepository();
                FileDescription dataRec = rep.Get(category, fileName);
                if (dataRec == null || IsNew(dataRec))
                {
                    dataRec = new FileDescription()
                    {
                        Category = category,
                        FileName = fileName
                    };
                }
                dataRec.Description = fileDescription;
                rep.Save(dataRec);

                return dataRec.Description;
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool IsNew(FileDescription dataRec)
        {
            return dataRec.pk == Guid.Empty;
        }
    }

    public class FileUploadInfo
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileDescription { get; set; }
    }

    public class FileDescriptionRepository : _BaseRepositoryRec
    {
        public List<FileDescription> GetForCategory(string category)
        {
            var sql = GetBaseQuery().Where(GetCategoryWhereClause(), new { Category = category });

            return Fetch<FileDescription>(sql);
        }

        public FileDescription Get(string category, string file)
        {
            var sql = GetBaseQuery().Where(GetCategoryWhereClause(), new { Category = category });
            sql.Where(GetFileWhereClause(), new { File = file });

            return Fetch<FileDescription>(sql).FirstOrDefault();
        }

        public FileDescription Get(Guid key)
        {
            var sql = GetBaseQuery().Where(GetBaseWhereClause(), new { Key = key });

            return Fetch<FileDescription>(sql).FirstOrDefault();
        }

        public bool Save(FileDescription dataRec)
        {
            if (IsNew(dataRec))
            {
                return Insert(dataRec);
            }
            else
            {
                return Update(dataRec);
            }
        }

        bool Insert(FileDescription dataRec)
        {
            dataRec.pk = Guid.NewGuid();

            object result = InsertInstance(dataRec);
            if (result is Guid)
            {
                return (Guid)result == dataRec.pk;
            }

            return false;
        }

        bool Update(FileDescription dataRec)
        {
            return UpdateInstance(dataRec);
        }

        public bool Delete(FileDescription dataRec)
        {
            return DeleteInstance(dataRec);
        }

        var sql GetBaseQuery()
        {
            return new Sql(string.Format("SELECT * FROM {0}", FileDescription.DbTableName));
        }

        string GetBaseWhereClause()
        {
            return string.Format("{0}.pk = @Key", FileDescription.DbTableName);
        }

        string GetCategoryWhereClause()
        {
            return string.Format("{0}.category = @Category", FileDescription.DbTableName);
        }

        string GetFileWhereClause()
        {
            return string.Format("{0}.fileName = @File", FileDescription.DbTableName);
        }
    }

    [TableName(FileDescription.DbTableName)]
    [PrimaryKey("pk", AutoIncrement = false)]
    public class FileDescription : _BaseRepositoryRec
    {
        public const string DbTableName = "nsFileDescription";

        public string Category { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
    }

    public class FileDescriptionCollection
    {
        Hashtable ht;

        public FileDescriptionCollection(string category)
        {
            FileDescriptionRepository rep = new FileDescriptionRepository();
            List<FileDescription> dataList = rep.GetForCategory(category);

            ht = new Hashtable(dataList.Count + 1);
            foreach (FileDescription dataRec in dataList)
            {
                if (!ht.ContainsKey(dataRec.FileName))
                {
                    ht.Add(dataRec.FileName, dataRec);
                }
            }
        }

        public string GetFileDescription(string file)
        {
            return ht.ContainsKey(file) ? ((FileDescription)ht[file]).Description : string.Empty;
        }
    }
}
 
