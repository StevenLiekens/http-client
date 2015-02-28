﻿namespace Uri.Grammar
{
    using System.Diagnostics.Contracts;

    using Text.Scanning;

    public class Path : Element
    {
        private readonly bool isEmpty;

        private readonly bool isAbsolute;

        public Path(PathAbsoluteOrEmpty path, ITextContext context)
            : base(path.Data, context)
        {
            Contract.Requires(path != null);
            Contract.Requires(context != null);
            this.isEmpty = path.IsEmpty;
            this.isAbsolute = path.IsAbsolute;
        }

        public Path(PathAbsolute path, ITextContext context)
            : base(path.Data, context)
        {
            Contract.Requires(path != null);
            Contract.Requires(context != null);
            this.isEmpty = false;
            this.isAbsolute = true;
        }

        public Path(PathNoScheme path, ITextContext context)
            : base(path.Data, context)
        {
            Contract.Requires(path != null);
            Contract.Requires(context != null);
            this.isEmpty = false;
            this.isAbsolute = false;
        }

        public Path(PathRootless path, ITextContext context)
            : base(path.Data, context)
        {
            Contract.Requires(path != null);
            Contract.Requires(context != null);
            this.isEmpty = false;
            this.isAbsolute = false;
        }

        public Path(PathEmpty path, ITextContext context)
            : base(string.Empty, context)
        {
            Contract.Requires(path != null);
            Contract.Requires(context != null);
            this.isEmpty = false;
            this.isAbsolute = false;
        }

        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
        }

        public bool IsAbsolute
        {
            get
            {
                return this.isAbsolute;
            }
        }
    }
}