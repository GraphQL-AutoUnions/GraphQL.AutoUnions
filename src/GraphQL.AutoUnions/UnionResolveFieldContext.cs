namespace GraphQL.AutoUnions
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using GraphQL.Execution;
    using GraphQL.Instrumentation;
    using GraphQL.Types;
    using GraphQL.Validation;
    using GraphQLParser.AST;

    internal class UnionResolveFieldContext<TUnion> : IResolveFieldContext
    {
        private readonly IResolveFieldContext _target;
        private readonly IUnionCast<TUnion> _unionCast;
        private readonly IUnionValueAccessor<TUnion> _unionValueAccessor;

        public UnionResolveFieldContext(
            IResolveFieldContext target,
            IUnionCast<TUnion> unionCast,
            IUnionValueAccessor<TUnion> unionValueAccessor)
        {
            this._target = target ?? throw new ArgumentNullException(nameof(target));
            this._unionCast = unionCast ?? throw new ArgumentNullException(nameof(unionCast));
            this._unionValueAccessor = unionValueAccessor ?? throw new ArgumentNullException(nameof(unionValueAccessor));
        }
        
        /// <summary>
        /// Unwrapping of IOneOf value
        /// </summary>
        public object Source
        {
            get
            {
                if (!this._unionCast.TryCast(this._target.Source, out var source))
                {
                    throw new InvalidOperationError(
                        $"Invalid cast from '{this._target.Source?.GetType().Name ?? "null"}' to '${typeof(TUnion).Name}'");
                }
                
                return this._unionValueAccessor.Access(source);
            }
        }

        #region Forwarded to _target
        
        public IDictionary<string, object> UserContext => this._target.UserContext;
        public GraphQLField FieldAst => this._target.FieldAst;
        public FieldType FieldDefinition => this._target.FieldDefinition;
        public IObjectGraphType ParentType => this._target.ParentType;
        public IResolveFieldContext Parent => this._target.Parent;
        public IDictionary<string, ArgumentValue> Arguments => this._target.Arguments;
        public IDictionary<string, DirectiveInfo> Directives => this._target.Directives;
        public object RootValue => this._target.RootValue;
        public ISchema Schema => this._target.Schema;
        public GraphQLDocument Document => this._target.Document;
        public GraphQLOperationDefinition Operation => this._target.Operation;
        public Variables Variables => this._target.Variables;
        public CancellationToken CancellationToken => this._target.CancellationToken;
        public Metrics Metrics => this._target.Metrics;
        public ExecutionErrors Errors => this._target.Errors;
        public IEnumerable<object> Path => this._target.Path;
        public IEnumerable<object> ResponsePath => this._target.ResponsePath;
        public Dictionary<string, (GraphQLField Field, FieldType FieldType)> SubFields => this._target.SubFields;
        public IReadOnlyDictionary<string, object> InputExtensions => this._target.InputExtensions;
        public IDictionary<string, object> OutputExtensions => this._target.OutputExtensions;
        public IServiceProvider RequestServices => this._target.RequestServices;
        public IExecutionArrayPool ArrayPool => this._target.ArrayPool;
        public ClaimsPrincipal User => this._target.User;

        #endregion
    }
}