using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示在线零售系统可供销售的商品对象。
    /// </summary>
    public class Product : AggregateRoot
    {
        #region Properties

        private string name;
        /// <summary>
        /// 获取或设置商品的名称。
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;
        /// <summary>
        /// 获取或设置商品的描述信息。
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private decimal unitPrice;
        /// <summary>
        /// 获取或设置商品的单价。
        /// </summary>
        /// <remarks>
        /// 在实际的销售系统中，商品单价并不是一个固定的值，它可以是
        /// 一个估价，也可以是一个加权平均值。这就需要涉及到库存管理系统，
        /// 本案例对这部分内容不作演示。
        /// </remarks>
        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        private string imageUrl;
        /// <summary>
        /// 获取或设置用于表述商品外观的图片的URL地址。
        /// </summary>
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        private bool isFeatured;
        /// <summary>
        /// 获取或设置一个<see cref="Boolean"/>值，该值表述当前商品
        /// 是否为特色商品。
        /// </summary>
        public bool IsFeatured
        {
            get { return isFeatured; }
            set { isFeatured = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 返回表示当前Object的字符串。
        /// </summary>
        /// <returns>表示当前对象的字符串。</returns>
        public override string ToString()
        {
            return name;
        }
        #endregion
    }
}
