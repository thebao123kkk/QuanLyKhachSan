using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class DashBoardBLL
    {
        private DashBoardDAL dal = new DashBoardDAL();

        public (int soDangO, int tongPhong) LayPhongDangO()
        {
            return dal.GetPhongDangO();
        }

        public int LaySoCheckIns(DateTime date)
        {
            return dal.GetCheckIns(date);
        }

        public int LaySoCheckOuts(DateTime date)
        {
            return dal.GetCheckOuts(date);
        }

        public int LaySoPhongBan()
        {
            return dal.GetPhongBan();
        }
    }
}
