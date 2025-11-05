using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pharma263.Persistence.Interceptors
{
    public class TaggedQueryCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            ManipulateCommand(command);

            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            ManipulateCommand(command);

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }
        private static readonly Regex _tableAliasRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))",
                RegexOptions.Multiline |
                RegexOptions.IgnoreCase |
                RegexOptions.Compiled);

        private static void ManipulateCommand(DbCommand command)
        {
            if (
                command.CommandText.IndexOf("-- NOLOCK", StringComparison.Ordinal) > -1 &&
                command.CommandText.IndexOf("DELETE ", StringComparison.OrdinalIgnoreCase) == -1
            )
            {
                command.CommandText =
                _tableAliasRegex.Replace(
                    command.CommandText,
                    "${tableAlias} WITH (NOLOCK)");
            }
        }
    }
}
