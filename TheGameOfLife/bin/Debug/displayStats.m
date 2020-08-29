%A script to show the number of insects during the lifetime of a program
table = load('TheGoLStats.csv');

GrnAverage = mean(table(:,2));
LadAverage = mean(table(:,3));

x  = table(:,1);
y1 = [table(:,2) table(:,3)];
y2 = [ones(size(x))*GrnAverage ones(size(x))*LadAverage];

figure;
plot(x,y1);
hold on;
line(x,y2(:,1),'Color','red','LineStyle','--');
line(x,y2(:,2),'Color','green','LineStyle','--');
xlim([0 max(x)]);
xlabel('Timesteps','FontSize',12,'FontWeight','normal','Color','k')
ylabel('Number of insects','FontSize',12,'FontWeight','normal','Color','k')
title('Insect population on each timestep')
legend('Greenflies','Ladybirds','Average greenflies number','Average ladybirds number');
hold off;