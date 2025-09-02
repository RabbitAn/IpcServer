using SqlSugar;

namespace IpcServer.Domain.Entities;


    [SugarTable("recipe_stations", "public")]
    public class StationRecipe:BaseEntity
    {
         /// <summary>
        /// 主配方ID
        /// </summary>
        [SugarColumn(ColumnName = "master_recipe_id", IsNullable = false)]
        public Guid MasterRecipeId { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        [SugarColumn(ColumnName = "workstation_id", IsNullable = true)]
        public Guid? WorkstationId { get; set; }

        /// <summary>
        /// 工位编号
        /// </summary>
        [SugarColumn(ColumnName = "station_code", Length = 50, IsNullable = false)]
        public string StationCode { get; set; } = string.Empty;

        /// <summary>
        /// 工位名称
        /// </summary>
        [SugarColumn(ColumnName = "station_name", Length = 100, IsNullable = false)]
        public string StationName { get; set; } = string.Empty;

        /// <summary>
        /// 操作类别（如扫描、拧紧、测试、包装等）
        /// </summary>
        [SugarColumn(ColumnName = "operation_type", Length = 50, IsNullable = true)]
        public string? OperationType { get; set; }

        /// <summary>
        /// 工序名称（具体工序描述）
        /// </summary>
        [SugarColumn(ColumnName = "process_name", Length = 100, IsNullable = true)]
        public string? ProcessName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity", IsNullable = false)]
        public int Quantity { get; set; } = 0;

        /// <summary>
        /// 工序步骤序号
        /// </summary>
        [SugarColumn(ColumnName = "process_step", IsNullable = false)]
        public int ProcessStep { get; set; } = 1;

        /// <summary>
        /// 枪号
        /// </summary>
        [SugarColumn(ColumnName = "gun_number", Length = 20, IsNullable = true)]
        public string? GunNumber { get; set; }

        /// <summary>
        /// 程序号
        /// </summary>
        [SugarColumn(ColumnName = "program_number", Length = 20, IsNullable = true)]
        public string? ProgramNumber { get; set; }

        /// <summary>
        /// 物料PN
        /// </summary>
        [SugarColumn(ColumnName = "material_pn", Length = 100, IsNullable = true)]
        public string? MaterialPn { get; set; }

        /// <summary>
        /// 套筒号
        /// </summary>
        [SugarColumn(ColumnName = "socket_number", Length = 20, IsNullable = true)]
        public string? SocketNumber { get; set; }

        /// <summary>
        /// 条码规则
        /// </summary>
        [SugarColumn(ColumnName = "barcode_rule", Length = 200, IsNullable = true)]
        public string? BarcodeRule { get; set; }

        /// <summary>
        /// 排序号（执行顺序控制）
        /// </summary>
        [SugarColumn(ColumnName = "sort_order", IsNullable = false)]
        public int SortOrder { get; set; } = 1;

        /// <summary>
        /// 工位节拍（单位：秒）
        /// </summary>
        [SugarColumn(ColumnName = "station_cycle_time", IsNullable = false)]
        public int StationCycleTime { get; set; } = 0;

        /// <summary>
        /// 相机编号
        /// </summary>
        [SugarColumn(ColumnName = "camera_number", Length = 20, IsNullable = true)]
        public string? CameraNumber { get; set; }

        /// <summary>
        /// 允许返工次数
        /// </summary>
        [SugarColumn(ColumnName = "rework_times", IsNullable = false)]
        public int ReworkTimes { get; set; } = 0;

        /// <summary>
        /// 是否需要校验
        /// </summary>
        [SugarColumn(ColumnName = "need_verification", IsNullable = false)]
        public bool NeedVerification { get; set; } = true;

        /// <summary>
        /// 物料类别
        /// </summary>
        [SugarColumn(ColumnName = "material_category", Length = 50, IsNullable = true)]
        public string? MaterialCategory { get; set; }

        /// <summary>
        /// 角度范围
        /// </summary>
        [SugarColumn(ColumnName = "angle_range", Length = 50, IsNullable = true)]
        public string? AngleRange { get; set; }

        /// <summary>
        /// 扭矩范围
        /// </summary>
        [SugarColumn(ColumnName = "torque_range", Length = 50, IsNullable = true)]
        public string? TorqueRange { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "created_at", IsNullable = false)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnName = "updated_at", IsNullable = false)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
