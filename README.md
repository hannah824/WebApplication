# 足球比赛比分控制系统

这是一个通过 TDD (测试驱动开发) 方法实现的足球比赛比分控制系统，用于管理和显示足球比赛的比分情况。

## 功能概述

系统提供以下功能：

- 记录主队进球 (HomeGoal)
- 记录客队进球 (AwayGoal)
- 进入下一个比赛阶段 (NextPeriod)
- 取消主队进球 (HomeCancel)
- 取消客队进球 (AwayCancel)
- 以用户友好的格式显示当前比分

## 比分格式

比分显示格式为：`主队得分:客队得分 (比赛阶段)`

例如：
- "1:0 (First Half)" - 上半场，主队1分，客队0分
- "2:1 (Second Half)" - 下半场，主队2分，客队1分

## 数据存储

比赛数据存储在 Match 对象中，使用特殊的字符串格式表示比赛结果：

- "H" 表示主队进球
- "A" 表示客队进球
- ";" 表示进入下一阶段

例如：
- "HAH" 表示上半场2:1（主队进球，客队进球，主队进球）
- "HAH;" 表示下半场2:1（上半场结束后的得分）

## 使用示例

```csharp
// 创建控制器
var controller = new MatchController();
int matchId = 91;

// 主队进球
string result = controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);
// 显示 "1:0 (First Half)"

// 客队进球
result = controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);
// 显示 "1:1 (First Half)"

// 进入下半场
result = controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);
// 显示 "1:1 (Second Half)"
```

## 异常处理

当尝试取消与最后一个进球类型不匹配的进球时，系统会抛出 `UpdateMatchResultException` 异常。

## 分号处理

当取消操作遇到分号(;)时，系统会保留分号，但会查找并删除最靠近分号的相应类型的进球标记。这确保了比赛阶段的记录（由分号表示）不会被取消操作影响，同时仍然能够取消先前阶段的进球。

## 项目结构

- `MatchController.cs` - 核心控制器类
- `Match.cs` - 比赛数据模型
- `MatchControllerTests.cs` - 单元测试
