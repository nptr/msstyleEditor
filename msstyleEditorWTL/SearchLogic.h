#pragma once

struct SearchProperties;
struct TreeItemData;

bool ContainsProperty(const SearchProperties& search, TreeItemData* treeItemData);
bool ContainsName(LPCWSTR str, TreeItemData* treeItemData);