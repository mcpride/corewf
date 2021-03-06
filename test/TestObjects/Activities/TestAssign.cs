﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using Microsoft.CoreWf;
using Microsoft.CoreWf.Statements;
using Test.Common.TestObjects.Utilities.Validation;
using System.Collections.Generic;
using Microsoft.CoreWf.Expressions;
using Test.Common.TestObjects.Activities.Tracing;

namespace Test.Common.TestObjects.Activities
{
    public class TestAssign<T> : TestActivity
    {
        private TestActivity _locationActivity;
        private TestActivity _valueActivity;

        public TestAssign()
        {
            this.ProductActivity = new Assign<T>();
        }

        public TestAssign(string displayName)
            : this()
        {
            this.DisplayName = displayName;
        }

        private Assign<T> ProductAssign
        {
            get
            {
                return (Assign<T>)this.ProductActivity;
            }
        }

        // Assign<T>.To
        public Variable<T> ToVariable
        {
            set { this.ProductAssign.To = new OutArgument<T>(value); }
        }

        public Expression<Func<ActivityContext, T>> ToExpression
        {
            set { this.ProductAssign.To = new OutArgument<T>(value); }
        }

        public TestActivity ToLocation
        {
            set
            {
                Activity<Location<T>> we = value.ProductActivity as Activity<Location<T>>;

                if (we == null)
                {
                    throw new Exception("TestActivity should be for Activity<Location<T>> for conversion");
                }

                this.ProductAssign.To = we;
                _locationActivity = value;
            }
        }

        // Assign<T>.Value
        public T Value
        {
            set { this.ProductAssign.Value = new InArgument<T>(value); }
        }

        public Variable<T> ValueVariable
        {
            set { this.ProductAssign.Value = new InArgument<T>(value); }
        }

        public Expression<Func<ActivityContext, T>> ValueExpression
        {
            set { this.ProductAssign.Value = new InArgument<T>(value); }
        }

        public TestActivity ValueActivity
        {
            set
            {
                this.ProductAssign.Value = new InArgument<T>((Activity<T>)value.ProductActivity);
                _valueActivity = value;
            }
        }

        internal override IEnumerable<TestActivity> GetChildren()
        {
            if (_valueActivity != null && !(_valueActivity.ExpectedOutcome == Outcome.None))
                yield return _valueActivity;

            if (_locationActivity != null && !(_locationActivity.ExpectedOutcome == Outcome.None))
                yield return _locationActivity;
        }
    }
}
