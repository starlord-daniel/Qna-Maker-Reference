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
            await context.PostAsync("What do you want to do?");
            context.Wait(QnADialog);
        }

        private async Task QnADialog(IDialogContext context, IAwaitable<object> result)
        {
            var activityResult = await result as Activity;
            var query = activityResult.Text;

            var qnaResult = QnaApi.GetFirstQnaAnswer(query);

            if (qnaResult == null)
            {
                await context.PostAsync("I can't answer that.");
            }
            else
            {
                await context.PostAsync(qnaResult.answers[0].answer);
            }
                

            context.Wait(MessageReceivedAsync);
        }

    }
}