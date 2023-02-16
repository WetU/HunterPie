﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record UserAccountResponse(
    [property: JsonProperty("username")] string Username,
    [property: JsonProperty("avatar_url")] string AvatarUrl,
    [property: JsonProperty("rating")] string Rating,
    [property: JsonProperty("badges")] List<object> Badges,
    [property: JsonProperty("is_supporter")] bool IsSupporter
);