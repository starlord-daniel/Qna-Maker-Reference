using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using QnABot.API;

namespace QnABot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // Prompt text
            await context.PostAsync("Was möchtest du wissen?");
            context.Wait(QnADialog);
        }

        private async Task MessageContinueAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // Prompt text
            await context.PostAsync("Kann ich dir noch weiterhelfen?");
            context.Wait(QnADialog);
        }

        private async Task DialogEndAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Ich hoffe, ich konnte dir helfen. Einen schönen Tag noch!");

            await this.StartAsync(context);
        }

        private async Task QnADialog(IDialogContext context, IAwaitable<object> result)
        {
            var activityResult = await result as Activity;
            var query = activityResult.Text;

            if (query.ToLower() == "nein")
            {
                context.Wait(DialogEndAsync);
            }

            var qnaResult = QnaApi.GetFirstQnaAnswer(query);

            if (qnaResult == null)
            {
                await context.PostAsync("Das kann ich leider nicht beantworten.");
            }
            else
            {
                await context.PostAsync(qnaResult.answers[0].answer);
            }
                

            context.Wait(MessageContinueAsync);
        }

    }
}