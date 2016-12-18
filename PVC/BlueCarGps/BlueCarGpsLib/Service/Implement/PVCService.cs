using BlueCarGpsLib.Data;
using BlueCarGpsLib.Service.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Data.Repository.Interface;
using BlueCarGpsLib.Data.Repository.Implement;
using BlueCarGpsLib.Model.Search;
using BlueCarGpsLib.Helper;

namespace BlueCarGpsLib.Service.Implement
{
    public class PVCService : ServiceBase, IPVCService
    {

        private IPVCRepository rep;
        public PVCService(string dbString) : base(dbString) {
            rep = new PVCRepository(this.Context, this.User);
        }
        public PVCService(string dbString, User user) : base(dbString, user) {
            rep = new PVCRepository(this.Context, this.User);
        }

        public List<PVC> All()
        {
            return rep.GetAll();
        }

        public bool Create(PVC pvc)
        {
            return rep.Create(pvc);
        }

        public bool Update(PVC pvc)
        {
            //string salt=string.IsNullOrEmpty( pvc.pwdSalt) ? pvc.GenSalt() : pvc.pwdSalt;
            //pvc.pwd = MD5Helper.Encryt(string.Format("{0}{1}", pvc.pwd, pvc.pwdSalt));
            return rep.Update(pvc);
        }

        public PVC FindById(int id)
        {
            DataContext dc = new DataContext(this.DbString);

            return dc.Context
                .GetTable<PVC>()
                .FirstOrDefault(s => s.id.Equals(id));
        }
        public IQueryable<PVC> Search(PVCSearchModel searchModel)
        {
            return rep.Search(searchModel);
        }

        public bool DeleteById(int id)
        {
            return rep.DeleteById(id);
        }
    }
}
