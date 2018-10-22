#pragma once

struct SearchProperties;
struct ItemData;

bool ContainsProperty(const SearchProperties& search, ItemData* treeItemData);
bool ContainsName(LPCWSTR str, ItemData* treeItemData);