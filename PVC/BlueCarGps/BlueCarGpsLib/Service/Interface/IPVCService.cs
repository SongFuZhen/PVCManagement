using BlueCarGpsLib.Data;
using BlueCarGpsLib.Model.Message;
using BlueCarGpsLib.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueCarGpsLib.Service.Interface
{
   public interface IPVCService
    {
        PVC FindById(int id);

        List<PVC> All();

        bool Create(PVC user);

        bool Update(PVC user);

        IQueryable<PVC> Search(PVCSearchModel searchModel);

        bool DeleteById(int id);
    }
}
