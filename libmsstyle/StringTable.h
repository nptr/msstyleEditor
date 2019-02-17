#pragma once

#include <memory>
#include <string>
#include <map>

namespace libmsstyle
{
    class StringTable
    {
    public:
        typedef std::map<int32_t, std::string>::iterator StringTableIterator;

        StringTable();
        ~StringTable();

        StringTableIterator begin();
        StringTableIterator end();
        StringTableIterator find(int32_t id) const;
        size_t size() const;

        void Set(int id, const std::string& resourceString);
        void Remove(int id);

    private:
        class Impl;
        std::shared_ptr<Impl> impl;
    };
}

