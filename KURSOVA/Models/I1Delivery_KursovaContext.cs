using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KURSOVA.Models
{
    public partial class I1Delivery_KursovaContext : DbContext
    {
        public I1Delivery_KursovaContext()
        {
        }

        public I1Delivery_KursovaContext(DbContextOptions<I1Delivery_KursovaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Courier> Courier { get; set; }
        public virtual DbSet<CourierType> CourierType { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<DishType> DishType { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderLine> OrderLine { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RestaurantDishRelation> RestaurantDishRelation { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayment { get; set; }
        public virtual DbSet<Street> Street { get; set; }
        public virtual DbSet<UnitOfMeasure> UnitOfMeasure { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<VCourier> VCourier { get; set; }
        public virtual DbSet<VDish> VDish { get; set; }
        public virtual DbSet<VInvoice> VInvoice { get; set; }
        public virtual DbSet<VLoctation> VLoctation { get; set; }
        public virtual DbSet<VMenu> VMenu { get; set; }
        public virtual DbSet<VOrder> VOrder { get; set; }
        public virtual DbSet<VOrderDetails> VOrderDetails { get; set; }
        public virtual DbSet<VRestaurant> VRestaurant { get; set; }
        public virtual DbSet<VSalaryPayment> VSalaryPayment { get; set; }
        public virtual DbSet<VUsers> VUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Account__5E55825B33E36C14")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasIndex(e => e.City1)
                    .HasName("UQ__City__AEC4A06DA1E844FE")
                    .IsUnique();

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.City1)
                    .IsRequired()
                    .HasColumnName("City")
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Courier>(entity =>
            {
                entity.Property(e => e.CourierId).HasColumnName("CourierID");

                entity.Property(e => e.CourierTypeId).HasColumnName("CourierTypeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasMaxLength(20);

                entity.HasOne(d => d.CourierType)
                    .WithMany(p => p.Courier)
                    .HasForeignKey(d => d.CourierTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Courier__Courier__571DF1D5");
            });

            modelBuilder.Entity<CourierType>(entity =>
            {
                entity.Property(e => e.CourierTypeId).HasColumnName("CourierTypeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasMaxLength(20);
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.Property(e => e.DeliveryId).HasColumnName("DeliveryID");

                entity.Property(e => e.CourierId).HasColumnName("CourierID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Weight).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Courier)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.CourierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Delivery__Courie__17036CC0");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Delivery__Locati__17F790F9");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Delivery__OrderI__160F4887");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.Cost).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DishTypeId).HasColumnName("DishTypeID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnitOfMeasureId).HasColumnName("UnitOfMeasureID");

                entity.HasOne(d => d.DishType)
                    .WithMany(p => p.Dish)
                    .HasForeignKey(d => d.DishTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Dish__DishTypeID__4316F928");

                entity.HasOne(d => d.UnitOfMeasure)
                    .WithMany(p => p.Dish)
                    .HasForeignKey(d => d.UnitOfMeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Dish__UnitOfMeas__4222D4EF");
            });

            modelBuilder.Entity<DishType>(entity =>
            {
                entity.HasIndex(e => e.DishType1)
                    .HasName("UQ__DishType__2014DB75326D36F9")
                    .IsUnique();

                entity.Property(e => e.DishTypeId).HasColumnName("DishTypeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DishType1)
                    .IsRequired()
                    .HasColumnName("DishType")
                    .HasMaxLength(20);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasIndex(e => new { e.District1, e.CityId })
                    .HasName("UQ__District__F4CF6C26C5F4AAC4")
                    .IsUnique();

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.District1)
                    .IsRequired()
                    .HasColumnName("District")
                    .HasMaxLength(20);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District_City");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DeliveryPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.TotalAmt).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Invoice__OrderID__656C112C");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.BuildingNbr)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Room).HasMaxLength(10);

                entity.Property(e => e.StreetId).HasColumnName("StreetID");

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.StreetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Location_Street");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.TotalAmt).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__CustomerI__4CA06362");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.Property(e => e.OrderLineId).HasColumnName("OrderLineID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RestaurantDishRelationId).HasColumnName("RestaurantDishRelationID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderLine__Order__5070F446");

                entity.HasOne(d => d.RestaurantDishRelation)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.RestaurantDishRelationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderLine__Resta__5165187F");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Restaurant)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurant_Location");
            });

            modelBuilder.Entity<RestaurantDishRelation>(entity =>
            {
                entity.HasIndex(e => new { e.RestaurantId, e.DishId })
                    .HasName("UQ__Restaura__96CD78438D850C05")
                    .IsUnique();

                entity.Property(e => e.RestaurantDishRelationId).HasColumnName("RestaurantDishRelationID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.RestaurantDishRelation)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restauran__DishI__47DBAE45");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantDishRelation)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Restauran__Resta__46E78A0C");
            });

            modelBuilder.Entity<SalaryPayment>(entity =>
            {
                entity.Property(e => e.SalaryPaymentId).HasColumnName("SalaryPaymentID");

                entity.Property(e => e.CourierId).HasColumnName("CourierID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EndPeriodDate).HasColumnType("datetime");

                entity.Property(e => e.FineAmt)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PaymentAmt).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentForDeliveries).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Premium)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StartPeriodDate).HasColumnType("datetime");

                entity.HasOne(d => d.Courier)
                    .WithMany(p => p.SalaryPayment)
                    .HasForeignKey(d => d.CourierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SalaryPay__Couri__5EBF139D");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasIndex(e => new { e.Street1, e.DistrictId })
                    .HasName("UQ__Street__95BDFD711438F4D1")
                    .IsUnique();

                entity.Property(e => e.StreetId).HasColumnName("StreetID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Street1)
                    .IsRequired()
                    .HasColumnName("Street")
                    .HasMaxLength(30);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Street)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Street_District");
            });

            modelBuilder.Entity<UnitOfMeasure>(entity =>
            {
                entity.Property(e => e.UnitOfMeasureId).HasColumnName("UnitOfMeasureID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.UnitOfMeasure1)
                    .IsRequired()
                    .HasColumnName("UnitOfMeasure")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__UserID__1EA48E88");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__UserTypeID__1F98B2C1");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VCourier>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vCourier");

                entity.Property(e => e.CourierId).HasColumnName("CourierID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Surname).HasMaxLength(20);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VDish>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vDish");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DishId).HasColumnName("DishID");

                entity.Property(e => e.DishType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnitOfMeasure)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VInvoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vInvoice");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DeliveryPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OrderTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Surname).HasMaxLength(20);

                entity.Property(e => e.TotalAmt).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VLoctation>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vLoctation");

                entity.Property(e => e.BuildingNbr)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Room).HasMaxLength(10);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<VMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vMenu");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DishType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RestaurantDishRelationId).HasColumnName("RestaurantDishRelationID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnitOfMeasure)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vOrder");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TotalAmt).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VOrderDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vOrderDetails");

                entity.Property(e => e.DishDescription).HasMaxLength(100);

                entity.Property(e => e.DishType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LineAmt).HasColumnType("decimal(37, 4)");

                entity.Property(e => e.OrderDescription).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderLineId).HasColumnName("OrderLineID");

                entity.Property(e => e.PortionSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UnitOfMeasure)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VRestaurant>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vRestaurant");

                entity.Property(e => e.BuildingNbr)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.District)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VSalaryPayment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSalaryPayment");

                entity.Property(e => e.EndPeriodDate).HasColumnType("datetime");

                entity.Property(e => e.FineAmt).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PaymentAmt).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Premium).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalaryPaymentId).HasColumnName("SalaryPaymentID");

                entity.Property(e => e.StartPeriodDate).HasColumnType("datetime");

                entity.Property(e => e.Surname).HasMaxLength(20);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<VUsers>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vUsers");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
