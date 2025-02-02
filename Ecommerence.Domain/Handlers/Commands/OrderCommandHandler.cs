﻿using MediatR;
using Ecommerence.Domain.Bus;
using Ecommerence.Domain.Commands.Order;
using Ecommerence.Domain.Enums;
using Ecommerence.Domain.Events.Order;
using Ecommerence.Domain.Handlers.Common;
using Ecommerence.Domain.Interfaces.Repositories;
using Ecommerence.Domain.Interfaces.Repositories.Common;
using Ecommerence.Domain.Models;
using Ecommerence.Domain.Notifications;
using Ecommerence.Utils.Validations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerence.Domain.Handlers.Commands
{
    public class OrderCommandHandler : 
        CommandHandler, 
        IRequestHandler<CreateOrderCommand, Order>, 
        IRequestHandler<AddProductCommand>, 
        IRequestHandler<RemoveProductCommand>,
        IRequestHandler<ApproveOrderCommand>,
        IRequestHandler<CommitOrderCommand>,
        IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public OrderCommandHandler(
            IUnitOfWork uow, 
            IMediatorHandler bus, 
            INotificationHandler<DomainNotification> notifications,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository
        ) : base(uow, bus, notifications)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await GetOpenenedOrderFromUser(request.UserId);

            if (entity != null)
                return entity;

            var user = await userRepository.GetAsync(request.UserId);
            entity = new Order(user);

            await orderRepository.AddAsync(entity);

            Commit();
            await bus.InvokeAsync(new CreateOrderEvent(entity.Id, entity.Ticket, entity.Customer));

            return entity;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            request.IsLessThan(e => e.Quantity, 0, async () => await bus.InvokeDomainNotificationAsync("Invalid quantity."));

            if(!IsValidOperation())
                return Unit.Value;

            var order = await GetOpenenedOrderFromUser(request.UserId);

            if (order == null) 
                return Unit.Value;

            var product = await productRepository.FirstOrDefaultAsync(e => e.Id == request.ProductId);

            if (product == null)
                return Unit.Value;

            var orderItem = new OrderItem(product, request.Quantity);
            order.AddItem(orderItem);
            await orderRepository.UpdateAsync(order);
            Commit();
            await bus.InvokeAsync(new AddProductEvent(orderItem));

            return Unit.Value;
        }

        public async Task<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var order = await GetOpenenedOrderFromUser(request.UserId);

            if (order == null)
                return Unit.Value;

            order.RemoveItemByProductId(request.ProductId);
            await orderRepository.UpdateAsync(order);
            Commit();
            await bus.InvokeAsync(new RemoveProductEvent(order.Id, request.ProductId));

            return Unit.Value;
        }

        public async Task<Unit> Handle(ApproveOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FirstOrDefaultAsync(e => e.Id == request.OrderId);

            if (order == null)
                return Unit.Value;

            var user = await userRepository.GetAsync(request.UserId);

            order.Approve(user);
            await orderRepository.UpdateAsync(order);
            Commit();
            await bus.InvokeAsync(new ApproveOrderEvent(request.OrderId, request.UserId));

            return Unit.Value;
        }

        public async Task<Unit> Handle(CommitOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FirstOrDefaultAsync(e => e.Id == request.OrderId);

            if (order == null)
                return Unit.Value;

            var user = await userRepository.GetAsync(request.UserId);

            order.Commit(user);
            await orderRepository.UpdateAsync(order);
            Commit();
            await bus.InvokeAsync(new CommitOrderEvent(request.OrderId, request.UserId));

            return Unit.Value;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FirstOrDefaultAsync(e => e.Id == request.OrderId);

            if (order == null)
                return Unit.Value;

            var user = await userRepository.GetAsync(request.UserId);

            order.Cancel(user);
            await orderRepository.UpdateAsync(order);
            Commit();
            await bus.InvokeAsync(new CancelOrderEvent(request.OrderId, request.UserId));

            return Unit.Value;
        }

        private async Task<Order> GetOpenenedOrderFromUser(Guid userId) => await orderRepository.FirstOrDefaultAsync(e => e.Customer.Id == userId && e.Status == OrderStatus.Opened);
    }
}
