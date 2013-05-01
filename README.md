MoqMatcherExtractor
===================
`Moq` couldn't be used in one situation so I opted for manual mocking and written MoqMatcherExtractor to reuse `It.Is` matchers for manual mock setup.

But I ended up just refactoring matchers to return `Func<T, bool>` so I didn't need MoqMatcherExtractor. But I'm leaving it here as it's interesting, I hope.