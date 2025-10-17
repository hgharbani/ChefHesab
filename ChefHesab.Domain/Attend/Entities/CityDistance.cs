using KSC.Domain;

namespace Ksc.HR.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class CityDistance : IEntityBase<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CityId1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CityId2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Distancy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] RowVersion { get; set; }

    }
}
