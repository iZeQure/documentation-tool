using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationTool.Shared.Handlers
{
    public interface IDocumentHandler
    {
        /// <summary>
        /// Uploads a specific document.
        /// </summary>
        /// <returns>True if uploaded, otherwise false.</returns>
        public bool UploadDocumentation();

        /// <summary>
        /// Uploads a set of documents.
        /// </summary>
        /// <returns>True if the set was uploaded, otherwise false.</returns>
        public bool UploadMixedDocumentation();
    }
}
