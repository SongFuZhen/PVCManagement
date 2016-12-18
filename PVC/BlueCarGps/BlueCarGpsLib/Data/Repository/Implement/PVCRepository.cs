using BlueCarGpsLib.Data.Repository.Interface;

using BlueCarGpsLib.Model.UserAuth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueCarGpsLib.Model.Search;
using BlueCarGpsLib.Helper;

namespace BlueCarGpsLib.Data.Repository.Implement
{
    public class PVCRepository : RepositoryBase<PVC>, IPVCRepository
    {
        private BlurCarGpsDataContext context;
        public PVCRepository(IDataContextFactory context):base(context) {
            this.context = this._dataContextFactory.Context as BlurCarGpsDataContext;
        }

        public PVCRepository(IDataContextFactory context, User user) : base(context, user) {
            this.context = this._dataContextFactory.Context as BlurCarGpsDataContext;
        }

        public bool Create(PVC pvc)
        {
            try
            {
                this.context.GetTable<PVC>().InsertOnSubmit(pvc);
                this.context.SubmitChanges();
                return true;
            }catch
            {
                return false;
            }
        }
        public PVC FindById(int id)
        {
            return BaseQuery().FirstOrDefault(u => u.id.Equals(id));
        }
       
        public List<PVC> GetAll()
        {
            var q = this.context.PVC;
            return q == null ? new List<PVC>() : q.ToList();
        }

        public bool DeleteById(int id)
        {
            try
            {
                PVC pvc = FindById(id);
                if (pvc != null)
               { 
                    this.context.GetTable<PVC>().DeleteOnSubmit(pvc);
                    this.context.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        private IQueryable<PVC> BaseQuery()
        {
            try
            {
                if (this.user.roleTypeEnum == RoleType.Admin)
                {
                    return this.context.PVC;
                }
                else
                {
                    return this.context.PVC
                        .Where(c => c.id.Equals(this.user.id));
                }
            }
            catch (Exception)
            {
                return this.context.PVC;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pvc"></param>
        /// <returns></returns>
        public bool Update(PVC pvc)
        {
            try
            {
                PVC pvcs = FindById(pvc.id);//this.context.GetTable<Car>().FirstOrDefault(c => c.id.Equals(car.id));

                if (pvcs != null)
                {
                    pvcs.name = pvc.name;
                    pvcs.shipDate = pvc.shipDate;
                    pvcs.shipAmount = pvc.shipAmount;
                    pvcs.unit = pvc.unit;
                    pvcs.brand = pvc.brand;
                    // 通过PENum进行 分解， 填充batchNo， startTime， endTime
                    pvcs.peNum = pvc.peNum;
                    this.context.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<PVC> Search(PVCSearchModel searchModel)
        {
            IQueryable<PVC> pvcs = BaseQuery();

            if (!string.IsNullOrWhiteSpace(searchModel.name))
            {
                pvcs = pvcs.Where(c => c.name.Contains(searchModel.name.Trim()));
            }

            if (searchModel.shipDateFrom.HasValue)
            {
                pvcs = pvcs.Where(c => c.shipDate >= searchModel.shipDateFrom);
            }

            if (searchModel.shipDateTo.HasValue)
            {
                pvcs = pvcs.Where(c => c.shipDate <= searchModel.shipDateTo);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.peNum))
            {

                //pvcs = pvcs.Where(c => c.peNum.Contains(searchModel.peNum));

                try
                {
                    string batchNo = searchModel.peNum.Split('.')[0];
                    string timeStr = searchModel.peNum.Split('.')[1];

                    if (!string.IsNullOrWhiteSpace(batchNo))
                    {
                        pvcs = pvcs.Where(c => c.batchNo.Equals(batchNo));
                    }

                    if (!string.IsNullOrWhiteSpace(timeStr))
                    {
                        DateTime time = DateTime.ParseExact(timeStr, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                        //pvcs = pvcs.Where(c => c.startTime <= time).Where(c => c.endTime >= time);

                        pvcs = pvcs.Where(c => c.startTime <= time && c.endTime >= time);
                    }
                }
                catch (Exception)
                {
                    if (!string.IsNullOrWhiteSpace(searchModel.peNum))
                    {
                        pvcs = pvcs.Where(c => c.batchNo.Equals(searchModel.peNum));
                    }

                }
            }

            if (!string.IsNullOrWhiteSpace(searchModel.brand))
            {
                pvcs = pvcs.Where(c => c.brand.Contains(searchModel.brand));
            }


            //if (!string.IsNullOrWhiteSpace(searchModel.batchNo))
            //{
            //    pvcs = pvcs.Where(c => c.batchNo.Contains(searchModel.batchNo));
            //}

            //if (searchModel.startTimeFrom.HasValue)
            //{
            //    pvcs = pvcs.Where(c => c.startTime >= searchModel.startTimeFrom);
            //}

            //if (searchModel.startTimeTo.HasValue)
            //{
            //    pvcs = pvcs.Where(c => c.startTime <= searchModel.startTimeTo);
            //}

            //if (searchModel.endTimeFrom.HasValue)
            //{
            //    pvcs = pvcs.Where(c => c.endTime >= searchModel.endTimeFrom);
            //}

            //if (searchModel.endTimeTo.HasValue)
            //{
            //    pvcs = pvcs.Where(c => c.endTime <= searchModel.endTimeTo);
            //}

            if (searchModel.createdAtFrom.HasValue)
            {
                pvcs = pvcs.Where(c => c.createdAt >= searchModel.createdAtFrom);
            }

            if (searchModel.createdAtTo.HasValue)
            {
                pvcs = pvcs.Where(c => c.createdAt <= searchModel.createdAtTo);
            }

            return pvcs;
        }
    }
}
